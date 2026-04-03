using CommonTestUtilities.Requests;
using FluentValidation.TestHelper;
using ResidentialExpenses.Application.UseCases.People.Update;
using ResidentialExpenses.Exceptions;

namespace Validators.Test.People;

public class UpdatePersonValidatorTest
{
    private readonly UpdatePersonValidator _validator = new();

    [Fact]
    public void Success()
    {
        var request = RequestUpdatePersonJsonBuilder.Build();

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var request = RequestUpdatePersonJsonBuilder.Build();
        request.Name = string.Empty;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(ResourceErrorMessages.PERSON_NAME_EMPTY);
    }

    [Fact]
    public void Error_Age_Zero()
    {
        var request = RequestUpdatePersonJsonBuilder.Build();
        request.Age = 0;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Age)
            .WithErrorMessage(ResourceErrorMessages.PERSON_AGE_INVALID);
    }

    [Fact]
    public void Error_Age_Negative()
    {
        var request = RequestUpdatePersonJsonBuilder.Build();
        request.Age = -1;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Age)
            .WithErrorMessage(ResourceErrorMessages.PERSON_AGE_INVALID);
    }
}
