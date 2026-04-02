using FluentValidation;
using ResidentialExpenses.Application.SharedValidators;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Exceptions;

namespace ResidentialExpenses.Application.UseCases.Users.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(user => string.IsNullOrWhiteSpace(user.Email) == false, ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);

        RuleFor(user => user.NewPassword)
            .SetValidator(new PasswordValidator<RequestUpdateUserJson>())
            .When(user => string.IsNullOrWhiteSpace(user.NewPassword) == false);

        RuleFor(user => user.OldPassword)
            .NotEmpty()
            .WithMessage(ResourceErrorMessages.INVALID_PASSWORD)
            .When(user => string.IsNullOrWhiteSpace(user.NewPassword) == false);
    }
}
