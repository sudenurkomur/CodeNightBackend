using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Challenges.Queries.GetChallenges;

public record GetChallengesQuery(
    bool? IsActive = null,
    string? Type = null,
    string? Sort = null
) : IRequest<ApiResponse<List<ChallengeDto>>>;
