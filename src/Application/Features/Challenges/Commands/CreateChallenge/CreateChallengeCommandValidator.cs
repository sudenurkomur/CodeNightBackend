using CodeNight.Domain.Enums;
using FluentValidation;

namespace CodeNight.Application.Features.Challenges.Commands.CreateChallenge;

public class CreateChallengeCommandValidator : AbstractValidator<CreateChallengeCommand>
{
    public CreateChallengeCommandValidator()
    {
        RuleFor(x => x.ChallengeName)
            .NotEmpty().WithMessage("challenge_name is required.")
            .MaximumLength(200);

        RuleFor(x => x.ChallengeType)
            .NotEmpty().WithMessage("challenge_type is required.")
            .Must(t => Enum.TryParse<ChallengeType>(t, true, out _))
            .WithMessage("challenge_type must be one of: DAILY, WEEKLY, STREAK.");

        RuleFor(x => x.Condition)
            .NotEmpty().WithMessage("condition is required.");

        RuleFor(x => x.RewardPoints)
            .GreaterThan(0).WithMessage("reward_points must be greater than 0.");

        RuleFor(x => x.Priority)
            .GreaterThan(0).WithMessage("priority must be greater than 0.");
    }
}
