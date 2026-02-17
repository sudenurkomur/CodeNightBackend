using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using CodeNight.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Challenges.Queries.GetChallenges;

public class GetChallengesQueryHandler : IRequestHandler<GetChallengesQuery, ApiResponse<List<ChallengeDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetChallengesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<ChallengeDto>>> Handle(
        GetChallengesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Challenges.AsNoTracking().AsQueryable();

        if (request.IsActive.HasValue)
            query = query.Where(c => c.IsActive == request.IsActive.Value);

        if (!string.IsNullOrWhiteSpace(request.Type) && Enum.TryParse<ChallengeType>(request.Type, true, out var ct))
            query = query.Where(c => c.ChallengeType == ct);

        query = request.Sort?.ToLowerInvariant() switch
        {
            "priority_desc" => query.OrderByDescending(c => c.Priority),
            "reward_desc" => query.OrderByDescending(c => c.RewardPoints),
            "reward_asc" => query.OrderBy(c => c.RewardPoints),
            _ => query.OrderBy(c => c.Priority) // default: priority_asc
        };

        var challenges = await query
            .Select(c => new ChallengeDto
            {
                ChallengeId = c.ChallengeId,
                ChallengeName = c.ChallengeName,
                ChallengeType = c.ChallengeType.ToString(),
                Condition = c.Condition,
                RewardPoints = c.RewardPoints,
                Priority = c.Priority,
                IsActive = c.IsActive
            })
            .ToListAsync(cancellationToken);

        return new ApiResponse<List<ChallengeDto>>
        {
            Data = challenges,
            Meta = new MetaInfo()
        };
    }
}
