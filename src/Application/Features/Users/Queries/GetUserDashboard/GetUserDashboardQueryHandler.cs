using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using CodeNight.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Users.Queries.GetUserDashboard;

public class GetUserDashboardQueryHandler : IRequestHandler<GetUserDashboardQuery, UserDashboardDto>
{
    private readonly IApplicationDbContext _context;

    public GetUserDashboardQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDashboardDto> Handle(GetUserDashboardQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .AsNoTracking()
            .Include(u => u.UserState)
            .FirstOrDefaultAsync(u => u.UserId == request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException($"User with id {request.UserId} not found.");

        var latestAward = await _context.ChallengeAwards
            .AsNoTracking()
            .Where(ca => ca.UserId == request.UserId)
            .OrderByDescending(ca => ca.CreatedAt)
            .Select(ca => new ChallengeAwardDto
            {
                AwardId = ca.AwardId,
                AsOfDate = ca.AsOfDate,
                RewardPoints = ca.RewardPoints,
                CreatedAt = ca.CreatedAt,
                ChallengeName = ca.TriggeredChallenges
                    .Where(tc => tc.Status == TriggeredChallengeStatus.SELECTED)
                    .Select(tc => tc.Challenge.ChallengeName)
                    .FirstOrDefault() ?? string.Empty
            })
            .FirstOrDefaultAsync(cancellationToken);

        var badges = await _context.BadgeAwards
            .AsNoTracking()
            .Where(ba => ba.UserId == request.UserId)
            .Select(ba => new BadgeDto
            {
                BadgeId = ba.BadgeId,
                BadgeName = ba.Badge.BadgeName,
                Level = ba.Badge.Level.ToString(),
                AwardedAt = ba.AwardedAt
            })
            .ToListAsync(cancellationToken);

        var notifications = await _context.Notifications
            .AsNoTracking()
            .Where(n => n.UserId == request.UserId)
            .OrderByDescending(n => n.SentAt)
            .Take(20)
            .Select(n => new NotificationDto
            {
                NotificationId = n.NotificationId,
                Channel = n.Channel.ToString(),
                Message = n.Message,
                SentAt = n.SentAt
            })
            .ToListAsync(cancellationToken);

        return new UserDashboardDto
        {
            UserId = user.UserId,
            Name = user.Name,
            Surname = user.Surname,
            UserState = user.UserState != null
                ? new UserStateDto
                {
                    ListenMinutesToday = user.UserState.ListenMinutesToday,
                    UniqueTracksToday = user.UserState.UniqueTracksToday,
                    PlaylistAdditionsToday = user.UserState.PlaylistAdditionsToday,
                    SharesToday = user.UserState.SharesToday,
                    ListenMinutes7d = user.UserState.ListenMinutes7d,
                    Shares7d = user.UserState.Shares7d,
                    UniqueTracks7d = user.UserState.UniqueTracks7d,
                    ListenStreakDays = user.UserState.ListenStreakDays,
                    TotalPoints = user.UserState.TotalPoints
                }
                : null,
            LatestAward = latestAward,
            Badges = badges,
            Notifications = notifications
        };
    }
}
