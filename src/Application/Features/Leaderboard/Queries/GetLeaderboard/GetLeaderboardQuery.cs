using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Leaderboard.Queries.GetLeaderboard;

public record GetLeaderboardQuery(int Top = 10) : IRequest<List<LeaderboardEntryDto>>;
