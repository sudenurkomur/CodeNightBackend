using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Challenges.Commands.CreateChallenge;

public record CreateChallengeCommand(
    string ChallengeName,
    string ChallengeType,
    string Condition,
    long RewardPoints,
    int Priority,
    bool IsActive
) : IRequest<ApiResponse<ChallengeDto>>;
