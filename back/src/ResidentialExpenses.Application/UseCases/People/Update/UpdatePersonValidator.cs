using FluentValidation;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Exceptions;

namespace ResidentialExpenses.Application.UseCases.People.Update;

public class UpdatePersonValidator : AbstractValidator<RequestUpdatePersonJson>
{
    public UpdatePersonValidator()
    {
        RuleFor(person => person.Name).NotEmpty().WithMessage(ResourceErrorMessages.PERSON_NAME_EMPTY);
        RuleFor(person => person.Age).GreaterThan(0).WithMessage(ResourceErrorMessages.PERSON_AGE_INVALID);
    }
}
