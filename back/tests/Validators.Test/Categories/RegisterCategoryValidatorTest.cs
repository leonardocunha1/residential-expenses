using CommonTestUtilities.Requests;
using FluentValidation.TestHelper;
using ResidentialExpenses.Application.UseCases.Categories.Register;
using ResidentialExpenses.Communication.Enums;
using ResidentialExpenses.Exceptions;

namespace Validators.Test.Categories;

public class RegisterCategoryValidatorTest
{
    private readonly RegisterCategoryValidator _validator = new();

    [Fact]
    public void Success()
    {
        var request = RequestRegisterCategoryJsonBuilder.Build();

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_Description_Empty()
    {
        var request = RequestRegisterCategoryJsonBuilder.Build();
        request.Description = string.Empty;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage(ResourceErrorMessages.CATEGORY_DESCRIPTION_EMPTY);
    }

    [Fact]
    public void Error_Purpose_Invalid()
    {
        var request = RequestRegisterCategoryJsonBuilder.Build();
        request.Purpose = (CategoryPurposeJson)999;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Purpose)
            .WithErrorMessage(ResourceErrorMessages.CATEGORY_PURPOSE_INVALID);
    }
}
