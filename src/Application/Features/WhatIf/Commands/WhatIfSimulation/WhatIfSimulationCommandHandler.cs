using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using CodeNight.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.WhatIf.Commands.WhatIfSimulation;

public class WhatIfSimulationCommandHandler : IRequestHandler<WhatIfSimulationCommand, ApiResponse<WhatIfResultDto>>
{
    private readonly IApplicationDbContext _context;

    public WhatIfSimulationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<WhatIfResultDto>> Handle(
        WhatIfSimulationCommand request, CancellationToken cancellationToken)
    {
        var userState = await _context.UserStates
            .AsNoTracking()
            .FirstOrDefaultAsync(us => us.UserId == request.UserId, cancellationToken)
            ?? throw new KeyNotFoundException($"UserState for user {request.UserId} not found.");

        var activeChallenges = await _context.Challenges
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.Priority)
            .ToListAsync(cancellationToken);

        // Build baseline metrics
        var baselineMetrics = new Dictionary<string, long>
        {
            ["listen_minutes_today"] = userState.ListenMinutesToday,
            ["unique_tracks_today"] = userState.UniqueTracksToday,
            ["playlist_additions_today"] = userState.PlaylistAdditionsToday,
            ["shares_today"] = userState.SharesToday,
            ["listen_minutes_7d"] = userState.ListenMinutes7d,
            ["shares_7d"] = userState.Shares7d,
            ["unique_tracks_7d"] = userState.UniqueTracks7d,
            ["listen_streak_days"] = userState.ListenStreakDays
        };

        // Evaluate baseline
        var baseline = EvaluateChallenges(activeChallenges, baselineMetrics);

        // Build what-if metrics (apply delta)
        var whatIfMetrics = new Dictionary<string, long>(baselineMetrics);
        foreach (var (key, value) in request.Delta)
        {
            if (whatIfMetrics.ContainsKey(key))
                whatIfMetrics[key] += value;
        }

        // Evaluate what-if
        var whatIf = EvaluateChallenges(activeChallenges, whatIfMetrics);

        // Build explanation
        var deltaDescription = string.Join(", ", request.Delta.Select(d => $"{d.Key} +{d.Value}"));
        var explanation = $"{deltaDescription} ile ";
        if (whatIf.SelectedChallenge.HasValue && whatIf.SelectedChallenge != baseline.SelectedChallenge)
        {
            var selectedChallenge = activeChallenges.FirstOrDefault(c => c.ChallengeId == whatIf.SelectedChallenge);
            explanation += $"'{selectedChallenge?.ChallengeName ?? "bilinmeyen"}' tetiklendi; daha yüksek öncelik olduğu için seçildi.";
        }
        else if (whatIf.TriggeredChallenges.Count > baseline.TriggeredChallenges.Count)
        {
            explanation += "ek challenge tetiklendi ama seçim değişmedi.";
        }
        else
        {
            explanation += "herhangi bir değişiklik tetiklenmedi.";
        }

        return new ApiResponse<WhatIfResultDto>
        {
            Data = new WhatIfResultDto
            {
                Baseline = baseline,
                WhatIf = whatIf,
                Explanation = explanation
            },
            Meta = new MetaInfo { AsOfDate = request.AsOfDate.ToString("yyyy-MM-dd") }
        };
    }

    private static WhatIfScenario EvaluateChallenges(
        List<Domain.Entities.Challenge> challenges,
        Dictionary<string, long> metrics)
    {
        var triggered = new List<Guid>();

        foreach (var c in challenges)
        {
            if (EvaluateCondition(c.Condition, metrics))
                triggered.Add(c.ChallengeId);
        }

        Guid? selected = null;
        var suppressed = new List<Guid>();

        if (triggered.Count > 0)
        {
            // First triggered is already sorted by priority (lowest = highest priority)
            selected = triggered[0];
            suppressed = triggered.Skip(1).ToList();
        }

        return new WhatIfScenario
        {
            TriggeredChallenges = triggered,
            SelectedChallenge = selected,
            SuppressedChallenges = suppressed
        };
    }

    private static bool EvaluateCondition(string condition, Dictionary<string, long> metrics)
    {
        // Parse simple conditions like "listen_minutes_today >= 30"
        var parts = condition.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 3) return false;

        var field = parts[0];
        var op = parts[1];
        if (!long.TryParse(parts[2], out var threshold)) return false;
        if (!metrics.TryGetValue(field, out var value)) return false;

        return op switch
        {
            ">=" => value >= threshold,
            ">" => value > threshold,
            "<=" => value <= threshold,
            "<" => value < threshold,
            "==" => value == threshold,
            "!=" => value != threshold,
            _ => false
        };
    }
}
