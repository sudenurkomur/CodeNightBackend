using CodeNight.Application.Common;
using CodeNight.Application.DTOs;
using CodeNight.Application.Interfaces;
using CodeNight.Domain.Entities;
using CodeNight.Domain.Enums;
using MediatR;

namespace CodeNight.Application.Features.Challenges.Commands.CreateChallenge;

public class CreateChallengeCommandHandler : IRequestHandler<CreateChallengeCommand, ApiResponse<ChallengeDto>>
{
    private readonly IApplicationDbContext _context;

    public CreateChallengeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ApiResponse<ChallengeDto>> Handle(
        CreateChallengeCommand request, CancellationToken cancellationToken)
    {
        var challenge = new Challenge
        {
            ChallengeId = Guid.NewGuid(),
            ChallengeName = request.ChallengeName,
            ChallengeType = Enum.Parse<ChallengeType>(request.ChallengeType, true),
            Condition = request.Condition,
            RewardPoints = request.RewardPoints,
            Priority = request.Priority,
            IsActive = request.IsActive
        };

        _context.Challenges.Add(challenge);
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
