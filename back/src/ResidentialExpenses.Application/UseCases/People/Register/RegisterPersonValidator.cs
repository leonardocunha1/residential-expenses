using FluentValidation;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Exceptions;

namespace ResidentialExpenses.Application.UseCases.People.Register;

public class RegisterPersonValidator : AbstractValidator<RequestRegisterPersonJson>
{
    public RegisterPersonValidator()
    {
        RuleFor(person => person.Name).NotEmpty().WithMessage(ResourceErrorMessages.PERSON_NAME_EMPTY);
        RuleFor(person => person.Age).GreaterThan(0).WithMessage(ResourceErrorMessages.PERSON_AGE_INVALID);
    }
}
