using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Users.Queries.GetUserChallengeAwards;

public record GetUserChallengeAwardsQuery(
    Guid UserId,
    DateOnly? From = null,
    DateOnly? To = null,
    int Limit = 30,
    string? Cursor = null
) : IRequest<ApiResponse<List<ChallengeAwardDetailDto>>>;
