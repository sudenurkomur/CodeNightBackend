using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using MediatR;

namespace CodeNight.Application.Features.Challenges.Commands.UpdateChallenge;

public record UpdateChallengeCommand(
    Guid ChallengeId,
    string? ChallengeName = null,
    string? ChallengeType = null,
    string? Condition = null,
    long? RewardPoints = null,
    int? Priority = null,
    bool? IsActive = null
) : IRequest<ApiResponse<ChallengeDto>>;
