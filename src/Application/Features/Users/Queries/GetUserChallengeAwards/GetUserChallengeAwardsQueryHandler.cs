using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using CodeNight.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Users.Queries.GetUserChallengeAwards;

public class GetUserChallengeAwardsQueryHandler
    : IRequestHandler<GetUserChallengeAwardsQuery, ApiResponse<List<ChallengeAwardDetailDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetUserChallengeAwardsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<ChallengeAwardDetailDto>>> Handle(
        GetUserChallengeAwardsQuery request, CancellationToken cancellationToken)
    {
        var pagination = new CursorPaginationParams { Limit = Math.Clamp(request.Limit, 1, 100), Cursor = request.Cursor };
        var offset = pagination.GetOffset();

        var query = _context.ChallengeAwards
            .AsNoTracking()
            .Include(ca => ca.TriggeredChallenges)
            .Where(ca => ca.UserId == request.UserId);

        if (request.From.HasValue)
            query = query.Where(ca => ca.AsOfDate >= request.From.Value);
        if (request.To.HasValue)
            query = query.Where(ca => ca.AsOfDate <= request.To.Value);

        var awards = await query
            .OrderByDescending(ca => ca.CreatedAt)
            .Skip(offset)
            .Take(pagination.Limit)
            .Select(ca => new ChallengeAwardDetailDto
            {
                AwardId = ca.AwardId,
                UserId = ca.UserId,
                AsOfDate = ca.AsOfDate.ToString("yyyy-MM-dd"),
                RewardPoints = ca.RewardPoints,
                Timestamp = ca.CreatedAt,
                TriggeredChallenges = ca.TriggeredChallenges.Select(tc => tc.ChallengeId).ToList(),
                SelectedChallenge = ca.TriggeredChallenges
                    .Where(tc => tc.Status == TriggeredChallengeStatus.SELECTED)
                    .Select(tc => tc.ChallengeId)
                    .FirstOrDefault(),
                SuppressedChallenges = ca.TriggeredChallenges
                    .Where(tc => tc.Status == TriggeredChallengeStatus.SUPPRESSED)
                    .Select(tc => tc.ChallengeId)
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return new ApiResponse<List<ChallengeAwardDetailDto>>
        {
            Data = awards,
            Meta = new MetaInfo
            {
                NextCursor = CursorPaginationParams.EncodeCursor(offset, pagination.Limit, awards.Count)
            }
        };
    }
}
