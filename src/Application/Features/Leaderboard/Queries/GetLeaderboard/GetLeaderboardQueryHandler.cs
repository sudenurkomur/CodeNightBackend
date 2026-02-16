using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Leaderboard.Queries.GetLeaderboard;

public class GetLeaderboardQueryHandler : IRequestHandler<GetLeaderboardQuery, ApiResponse<List<LeaderboardEntryDto>>>
{
    private readonly IApplicationDbContext _context;

    public GetLeaderboardQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<List<LeaderboardEntryDto>>> Handle(
        GetLeaderboardQuery request, CancellationToken cancellationToken)
    {
        var pagination = new CursorPaginationParams { Limit = Math.Clamp(request.Limit, 1, 100), Cursor = request.Cursor };
        var offset = pagination.GetOffset();

        var leaderboard = await _context.UserStates
            .AsNoTracking()
            .OrderByDescending(us => us.TotalPoints)
            .ThenBy(us => us.UserId)
            .Skip(offset)
            .Take(pagination.Limit)
            .Select(us => new LeaderboardEntryDto
            {
                UserId = us.UserId,
                TotalPoints = us.TotalPoints
            })
            .ToListAsync(cancellationToken);

        for (int i = 0; i < leaderboard.Count; i++)
        {
            leaderboard[i].Rank = offset + i + 1;
        }

        return new ApiResponse<List<LeaderboardEntryDto>>
        {
            Data = leaderboard,
            Meta = new MetaInfo
            {
                AsOfDate = request.AsOfDate.ToString("yyyy-MM-dd"),
                NextCursor = CursorPaginationParams.EncodeCursor(offset, pagination.Limit, leaderboard.Count)
            }
        };
    }
}
