using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Leaderboard.Queries.GetLeaderboard;

public class GetLeaderboardQueryHandler : IRequestHandler<GetLeaderboardQuery, List<LeaderboardEntryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetLeaderboardQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LeaderboardEntryDto>> Handle(GetLeaderboardQuery request, CancellationToken cancellationToken)
    {
        var leaderboard = await _context.UserStates
            .AsNoTracking()
            .Include(us => us.User)
            .OrderByDescending(us => us.TotalPoints)
            .Take(request.Top)
            .Select(us => new LeaderboardEntryDto
            {
                UserId = us.UserId,
                Name = us.User.Name,
                Surname = us.User.Surname,
                City = us.User.City,
                TotalPoints = us.TotalPoints
            })
            .ToListAsync(cancellationToken);

        // Assign ranks
        for (int i = 0; i < leaderboard.Count; i++)
        {
            leaderboard[i].Rank = i + 1;
        }

        return leaderboard;
    }
}
