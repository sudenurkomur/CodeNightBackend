using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using CodeNight.Domain.Constants;
using CodeNight.Domain.Entities;
using CodeNight.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Processing.Commands.RunProcessing;

public class RunProcessingCommandHandler : IRequestHandler<RunProcessingCommand, ProcessingResultDto>
{
    private readonly IApplicationDbContext _context;

    public RunProcessingCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProcessingResultDto> Handle(RunProcessingCommand request, CancellationToken cancellationToken)
    {
        var asOfDate = request.AsOfDate;
        var result = new ProcessingResultDto { AsOfDate = asOfDate };

        var users = await _context.Users
            .Include(u => u.UserState)
            .ToListAsync(cancellationToken);

        foreach (var user in users)
        {
            // Step 1: Calculate user state
            await CalculateUserState(user, asOfDate, cancellationToken);

            // Step 2: Evaluate challenges (idempotent - skip if already processed today)
            var existingAward = await _context.ChallengeAwards
                .AnyAsync(ca => ca.UserId == user.UserId && ca.AsOfDate == asOfDate, cancellationToken);

            if (existingAward)
                continue;

            var challengeResult = await EvaluateChallenges(user, asOfDate, cancellationToken);
            result.ChallengesTriggered += challengeResult.triggered;
            result.AwardsGiven += challengeResult.awarded;

            // Step 3: Badge awarding
            var badgesAwarded = await AwardBadges(user, cancellationToken);
            result.BadgesAwarded += badgesAwarded;

            // Step 4: Notifications
            result.NotificationsSent += challengeResult.notificationsSent;
        }

        await _context.SaveChangesAsync(cancellationToken);

        result.UsersProcessed = users.Count;
        return result;
    }

    private async Task CalculateUserState(User user, DateOnly asOfDate, CancellationToken ct)
    {
        // Get today's events
        var todayEvents = await _context.Events
            .Where(e => e.UserId == user.UserId && e.Date == asOfDate)
            .ToListAsync(ct);

        // Get 7-day window events
        var sevenDaysAgo = asOfDate.AddDays(-6);
        var weekEvents = await _context.Events
            .Where(e => e.UserId == user.UserId && e.Date >= sevenDaysAgo && e.Date <= asOfDate)
            .ToListAsync(ct);

        // Calculate streak: count consecutive days with listen events going backward from asOfDate
        var streakDays = await CalculateListenStreak(user.UserId, asOfDate, ct);

        // Calculate current total points from ledger
        var totalPoints = await _context.PointsLedgerEntries
            .Where(pl => pl.UserId == user.UserId)
            .SumAsync(pl => pl.PointsDelta, ct);

        var state = user.UserState;
        if (state == null)
        {
            state = new UserState { UserId = user.UserId };
            _context.UserStates.Add(state);
            user.UserState = state;
        }

        state.ListenMinutesToday = todayEvents.Sum(e => e.ListenMinutes);
        state.UniqueTracksToday = todayEvents.Sum(e => e.UniqueTracks);
        state.PlaylistAdditionsToday = todayEvents.Sum(e => e.PlaylistAdditions);
        state.SharesToday = todayEvents.Sum(e => e.Shares);
        state.ListenMinutes7d = weekEvents.Sum(e => e.ListenMinutes);
        state.Shares7d = weekEvents.Sum(e => e.Shares);
        state.UniqueTracks7d = weekEvents.Sum(e => e.UniqueTracks);
        state.ListenStreakDays = streakDays;
        state.TotalPoints = totalPoints;
    }

    private async Task<long> CalculateListenStreak(Guid userId, DateOnly asOfDate, CancellationToken ct)
    {
        var datesWithListening = await _context.Events
            .Where(e => e.UserId == userId && e.ListenMinutes > 0)
            .Select(e => e.Date)
            .Distinct()
            .OrderByDescending(d => d)
            .ToListAsync(ct);

        long streak = 0;
        var checkDate = asOfDate;

        foreach (var date in datesWithListening)
        {
            if (date == checkDate)
            {
                streak++;
                checkDate = checkDate.AddDays(-1);
            }
            else if (date < checkDate)
            {
                break;
            }
        }

        return streak;
    }

    private async Task<(int triggered, int awarded, int notificationsSent)> EvaluateChallenges(
        User user, DateOnly asOfDate, CancellationToken ct)
    {
        var state = user.UserState;
        if (state == null)
            return (0, 0, 0);

        var activeChallenges = await _context.Challenges
            .Where(c => c.IsActive)
            .OrderBy(c => c.Priority)
            .ToListAsync(ct);

        var triggeredChallenges = new List<Challenge>();

        foreach (var challenge in activeChallenges)
        {
            if (IsChallengeTriggered(challenge, state))
            {
                triggeredChallenges.Add(challenge);
            }
        }

        if (triggeredChallenges.Count == 0)
            return (0, 0, 0);

        // Select the highest priority challenge (lowest priority number)
        var selectedChallenge = triggeredChallenges.OrderBy(c => c.Priority).First();

        // Create decision for selected
        var selectedDecision = new ChallengeDecision
        {
            DecisionId = Guid.NewGuid(),
            Reason = DecisionReasons.Selected,
            CreatedAt = DateTime.UtcNow
        };
        _context.ChallengeDecisions.Add(selectedDecision);

        // Create challenge award
        var award = new ChallengeAward
        {
            AwardId = Guid.NewGuid(),
            UserId = user.UserId,
            DecisionId = selectedDecision.DecisionId,
            AsOfDate = asOfDate,
            RewardPoints = selectedChallenge.RewardPoints,
            CreatedAt = DateTime.UtcNow
        };
        _context.ChallengeAwards.Add(award);

        // Create triggered challenge records
        foreach (var challenge in triggeredChallenges)
        {
            var isSelected = challenge.ChallengeId == selectedChallenge.ChallengeId;
            var status = isSelected
                ? TriggeredChallengeStatus.SELECTED
                : TriggeredChallengeStatus.SUPPRESSED;

            _context.TriggeredChallenges.Add(new TriggeredChallenge
            {
                AwardId = award.AwardId,
                ChallengeId = challenge.ChallengeId,
                Status = status
            });
        }

        // Insert points ledger entry (idempotent via unique index source+source_ref)
        var ledgerExists = await _context.PointsLedgerEntries
            .AnyAsync(pl => pl.Source == PointSources.ChallengeReward && pl.SourceRef == award.AwardId, ct);

        if (!ledgerExists)
        {
            _context.PointsLedgerEntries.Add(new PointsLedgerEntry
            {
                LedgerId = Guid.NewGuid(),
                UserId = user.UserId,
                PointsDelta = selectedChallenge.RewardPoints,
                Source = PointSources.ChallengeReward,
                SourceRef = award.AwardId,
                CreatedAt = DateTime.UtcNow
            });

            // Update total points in user state
            if (user.UserState != null)
            {
                user.UserState.TotalPoints += selectedChallenge.RewardPoints;
            }
        }

        // Send notification for selected challenge
        int notificationsSent = 0;
        var message = string.Format(
            NotificationTemplates.ChallengeRewardEarned,
            selectedChallenge.ChallengeName,
            selectedChallenge.RewardPoints);

        _context.Notifications.Add(new Notification
        {
            NotificationId = Guid.NewGuid(),
            UserId = user.UserId,
            Channel = NotificationChannel.BiP,
            Message = message,
            SentAt = DateTime.UtcNow
        });
        notificationsSent++;

        return (triggeredChallenges.Count, 1, notificationsSent);
    }

    private static bool IsChallengeTriggered(Challenge challenge, UserState state)
    {
        // Parse condition string as "metric>=threshold" format
        // e.g., "listen_minutes_today>=60", "shares_7d>=10", "listen_streak_days>=7"
        var condition = challenge.Condition.Trim();

        var operators = new[] { ">=", "<=", ">", "<", "==" };
        string? op = null;
        string metricName = string.Empty;
        string thresholdStr = string.Empty;

        foreach (var operatorStr in operators)
        {
            var idx = condition.IndexOf(operatorStr, StringComparison.Ordinal);
            if (idx > 0)
            {
                op = operatorStr;
                metricName = condition[..idx].Trim();
                thresholdStr = condition[(idx + operatorStr.Length)..].Trim();
                break;
            }
        }

        if (op == null || !long.TryParse(thresholdStr, out var threshold))
            return false;

        var metricValue = GetMetricValue(metricName, state);

        return op switch
        {
            ">=" => metricValue >= threshold,
            "<=" => metricValue <= threshold,
            ">" => metricValue > threshold,
            "<" => metricValue < threshold,
            "==" => metricValue == threshold,
            _ => false
        };
    }

    private static long GetMetricValue(string metricName, UserState state)
    {
        return metricName.ToLowerInvariant() switch
        {
            "listen_minutes_today" => state.ListenMinutesToday,
            "unique_tracks_today" => state.UniqueTracksToday,
            "playlist_additions_today" => state.PlaylistAdditionsToday,
            "shares_today" => state.SharesToday,
            "listen_minutes_7d" => state.ListenMinutes7d,
            "shares_7d" => state.Shares7d,
            "unique_tracks_7d" => state.UniqueTracks7d,
            "listen_streak_days" => state.ListenStreakDays,
            "total_points" => state.TotalPoints,
            _ => 0
        };
    }

    private async Task<int> AwardBadges(User user, CancellationToken ct)
    {
        if (user.UserState == null)
            return 0;

        var totalPoints = user.UserState.TotalPoints;

        // Find badges whose condition (threshold) is met by total_points
        var eligibleBadges = await _context.Badges
            .Where(b => b.Condition <= totalPoints)
            .ToListAsync(ct);

        // Get already awarded badge IDs
        var alreadyAwardedBadgeIds = await _context.BadgeAwards
            .Where(ba => ba.UserId == user.UserId)
            .Select(ba => ba.BadgeId)
            .ToListAsync(ct);

        var newBadges = eligibleBadges
            .Where(b => !alreadyAwardedBadgeIds.Contains(b.BadgeId))
            .ToList();

        foreach (var badge in newBadges)
        {
            _context.BadgeAwards.Add(new BadgeAward
            {
                UserId = user.UserId,
                BadgeId = badge.BadgeId,
                AwardedAt = DateTime.UtcNow
            });

            // Send badge notification
            var badgeMessage = string.Format(
                NotificationTemplates.BadgeEarned,
                badge.BadgeName,
                badge.Level.ToString());

            _context.Notifications.Add(new Notification
            {
                NotificationId = Guid.NewGuid(),
                UserId = user.UserId,
                Channel = NotificationChannel.BiP,
                Message = badgeMessage,
                SentAt = DateTime.UtcNow
            });
        }

        return newBadges.Count;
    }
}
