using FluentValidation;

namespace CodeNight.Application.Features.Processing.Commands.RunProcessing;

public class RunProcessingCommandValidator : AbstractValidator<RunProcessingCommand>
{
    public RunProcessingCommandValidator()
    {
        RuleFor(x => x.AsOfDate)
            .NotEmpty()
            .WithMessage("AsOfDate is required.");
    }
}
