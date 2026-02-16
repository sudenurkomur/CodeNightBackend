using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using CodeNight.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CodeNight.Application.Features.Challenges.Commands.UpdateChallenge;

public class UpdateChallengeCommandHandler : IRequestHandler<UpdateChallengeCommand, ApiResponse<ChallengeDto>>
{
    private readonly IApplicationDbContext _context;

    public UpdateChallengeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<ChallengeDto>> Handle(
        UpdateChallengeCommand request, CancellationToken cancellationToken)
    {
        var challenge = await _context.Challenges
            .FirstOrDefaultAsync(c => c.ChallengeId == request.ChallengeId, cancellationToken)
            ?? throw new KeyNotFoundException($"Challenge with id {request.ChallengeId} not found.");

        if (request.ChallengeName is not null)
            challenge.ChallengeName = request.ChallengeName;

        if (request.ChallengeType is not null && Enum.TryParse<ChallengeType>(request.ChallengeType, true, out var ct))
            challenge.ChallengeType = ct;

        if (request.Condition is not null)
            challenge.Condition = request.Condition;

        if (request.RewardPoints.HasValue)
            challenge.RewardPoints = request.RewardPoints.Value;

        if (request.Priority.HasValue)
            challenge.Priority = request.Priority.Value;

        if (request.IsActive.HasValue)
            challenge.IsActive = request.IsActive.Value;

        await _context.SaveChangesAsync(cancellationToken);

        return new ApiResponse<ChallengeDto>
        {
            Data = new ChallengeDto
            {
                ChallengeId = challenge.ChallengeId,
                ChallengeName = challenge.ChallengeName,
                ChallengeType = challenge.ChallengeType.ToString(),
                Condition = challenge.Condition,
                RewardPoints = challenge.RewardPoints,
                Priority = challenge.Priority,
                IsActive = challenge.IsActive
            },
            Meta = new MetaInfo()
        };
    }
}
