using FluentValidation;

namespace DawnWeaver.Application.TimeManagement.Queries;

public class GetTimeManagementProposalQueryValidator : AbstractValidator<GetTimeManagementProposalQuery>
{
    public GetTimeManagementProposalQueryValidator()
    {
        RuleFor(e => e.DurationInMinutes)
            .NotEmpty()
            .GreaterThan(0);
    }
}