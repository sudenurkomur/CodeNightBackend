using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Leaderboard.Queries.GetLeaderboard;

public record GetLeaderboardQuery(
    DateOnly AsOfDate,
    int Limit = 10,
    string? Cursor = null
) : IRequest<ApiResponse<List<LeaderboardEntryDto>>>;
