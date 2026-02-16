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
    private readonly IApplicationDbContext _db;

    public GetUserChallengeAwardsQueryHandler(IApplicationDbContext db) => _db = db;

    public async Task<ApiResponse<List<ChallengeAwardDetailDto>>> Handle(
        GetUserChallengeAwardsQuery request, CancellationToken cancellationToken)
    {
        var pagination = new CursorPaginationParams { Limit = request.Limit, Cursor = request.Cursor };
        var offset = pagination.GetOffset();

        var query = _db.ChallengeAwards
            .Include(ca => ca.TriggeredChallenges)
            .Where(ca => ca.UserId == request.UserId);

        if (request.From.HasValue)
            query = query.Where(ca => ca.AsOfDate >= request.From.Value);

        if (request.To.HasValue)
            query = query.Where(ca => ca.AsOfDate <= request.To.Value);

        var awards = await query
            .OrderByDescending(ca => ca.AsOfDate)
            .ThenByDescending(ca => ca.CreatedAt)
            .Skip(offset)
            .Take(pagination.Limit)
            .ToListAsync(cancellationToken);

        var dtos = awards.Select(a => new ChallengeAwardDetailDto
        {
            AwardId = a.AwardId,
            UserId = a.UserId,
            AsOfDate = a.AsOfDate.ToString("yyyy-MM-dd"),
            TriggeredChallenges = a.TriggeredChallenges.Select(tc => tc.ChallengeId).ToList(),
            SelectedChallenge = a.TriggeredChallenges
                .Where(tc => tc.Status == TriggeredChallengeStatus.SELECTED)
                .Select(tc => tc.ChallengeId)
                .FirstOrDefault(),
            SuppressedChallenges = a.TriggeredChallenges
                .Where(tc => tc.Status == TriggeredChallengeStatus.SUPPRESSED)
                .Select(tc => tc.ChallengeId)
                .ToList(),
            RewardPoints = a.RewardPoints,
            Timestamp = a.CreatedAt
        }).ToList();

        return new ApiResponse<List<ChallengeAwardDetailDto>>
        {
            Data = dtos,
            Meta = new MetaInfo
            {
                NextCursor = CursorPaginationParams.EncodeCursor(offset, pagination.Limit, awards.Count)
            }
        };
    }
}
