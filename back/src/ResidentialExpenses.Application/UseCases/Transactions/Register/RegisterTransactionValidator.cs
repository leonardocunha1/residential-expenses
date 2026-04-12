using FluentValidation;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Exceptions;

namespace ResidentialExpenses.Application.UseCases.Transactions.Register;

public class RegisterTransactionValidator : AbstractValidator<RequestRegisterTransactionJson>
{
    public RegisterTransactionValidator()
    {
        RuleFor(t => t.Description).NotEmpty().WithMessage(ResourceErrorMessages.TRANSACTION_DESCRIPTION_EMPTY);
        RuleFor(t => t.Value).GreaterThan(0).WithMessage(ResourceErrorMessages.TRANSACTION_VALUE_MUST_BE_POSITIVE);
        RuleFor(t => t.Type).IsInEnum().WithMessage(ResourceErrorMessages.TRANSACTION_TYPE_INVALID);
        RuleFor(t => t.CategoryId).GreaterThan(0).WithMessage(ResourceErrorMessages.TRANSACTION_CATEGORY_NOT_FOUND);
        RuleFor(t => t.PersonId).GreaterThan(0).WithMessage(ResourceErrorMessages.PERSON_NOT_FOUND);
    }
}
