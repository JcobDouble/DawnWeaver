using DawnWeaver.Application.Common.Interfaces;
using FluentValidation;

namespace DawnWeaver.Application.Events.Commands.UpdateEvent;

public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
{
    public UpdateEventCommandValidator(IDateTime dateTime)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .NotNull();
        
        RuleFor(e => e.Title)
            .NotEmpty()
            .NotNull()
            .MinimumLength(5)
            .MaximumLength(50);

        RuleFor(e => e.Description)
            .MinimumLength(10)
            .MaximumLength(500);

        RuleFor(e => e.StartDate)
            .GreaterThan(_ => dateTime.Now)
            .WithMessage("Event date must be in the future.");

        RuleFor(e => e.DurationInMinutes)
            .NotEmpty()
            .NotNull()
            .GreaterThan(0)
            .LessThan(1440)
            .WithMessage("Event duration must be specified.");
    }
}