using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Dashboard.Queries.GetDashboard;

public class GetDashboardQueryHandler : IRequestHandler<GetDashboardQuery, ApiResponse<DashboardDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDashboardQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<DashboardDto>> Handle(GetDashboardQuery request, CancellationToken cancellationToken)
    {
        // Users preview with total points
        var usersPreview = await _context.Users
            .AsNoTracking()
            .Include(u => u.UserState)
            .OrderByDescending(u => u.UserState != null ? u.UserState.TotalPoints : 0)
            .Take(25)
            .Select(u => new UserDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Surname = u.Surname,
                City = u.City,
                Role = u.Role.ToString(),
                TotalPoints = u.UserState != null ? u.UserState.TotalPoints : 0
            })
            .ToListAsync(cancellationToken);

        // Leaderboard top 10
        var leaderboard = await _context.UserStates
            .AsNoTracking()
            .OrderByDescending(us => us.TotalPoints)
            .ThenBy(us => us.UserId)
            .Take(10)
            .Select(us => new LeaderboardEntryDto
            {
                UserId = us.UserId,
                TotalPoints = us.TotalPoints
            })
            .ToListAsync(cancellationToken);

        for (int i = 0; i < leaderboard.Count; i++)
            leaderboard[i].Rank = i + 1;

        // Top genre distribution (from events on asOfDate)
        var topGenres = await _context.Events
            .AsNoTracking()
            .Where(e => e.Date == request.AsOfDate)
            .GroupBy(e => e.TopGenre)
            .Select(g => new TopGenreDto
            {
                Genre = g.Key.ToString(),
                Count = g.Count()
            })
            .OrderByDescending(g => g.Count)
            .ToListAsync(cancellationToken);

        return new ApiResponse<DashboardDto>
        {
            Data = new DashboardDto
            {
                UsersPreview = usersPreview,
                LeaderboardTop10 = leaderboard,
                TopGenreDistribution = topGenres
            },
            Meta = new MetaInfo { AsOfDate = request.AsOfDate.ToString("yyyy-MM-dd") }
        };
    }
}
