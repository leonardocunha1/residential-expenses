using FluentValidation;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Exceptions;

namespace ResidentialExpenses.Application.UseCases.Categories.Register;

public class RegisterCategoryValidator : AbstractValidator<RequestRegisterCategoryJson>
{
    public RegisterCategoryValidator()
    {
        RuleFor(category => category.Description).NotEmpty().WithMessage(ResourceErrorMessages.CATEGORY_DESCRIPTION_EMPTY);
        RuleFor(category => category.Purpose).IsInEnum().WithMessage(ResourceErrorMessages.CATEGORY_PURPOSE_INVALID);
    }
}
