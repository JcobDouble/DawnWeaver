using DawnWeaver.Application.Common.Interfaces;
using FluentValidation;

namespace DawnWeaver.Application.Events.Commands.AddEvent;

public class AddEventCommandValidator : AbstractValidator<AddEventCommand>
{
    public AddEventCommandValidator(IDateTime dateTime)
    {
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