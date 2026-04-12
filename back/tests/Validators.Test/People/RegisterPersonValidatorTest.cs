using CommonTestUtilities.Requests;
using FluentValidation.TestHelper;
using ResidentialExpenses.Application.UseCases.People.Register;
using ResidentialExpenses.Exceptions;

namespace Validators.Test.People;

public class RegisterPersonValidatorTest
{
    private readonly RegisterPersonValidator _validator = new();

    [Fact]
    public void Success()
    {
        var request = RequestRegisterPersonJsonBuilder.Build();

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var request = RequestRegisterPersonJsonBuilder.Build();
        request.Name = string.Empty;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(ResourceErrorMessages.PERSON_NAME_EMPTY);
    }

    [Fact]
    public void Error_Age_Zero()
    {
        var request = RequestRegisterPersonJsonBuilder.Build();
        request.Age = 0;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Age)
            .WithErrorMessage(ResourceErrorMessages.PERSON_AGE_INVALID);
    }

    [Fact]
    public void Error_Age_Negative()
    {
        var request = RequestRegisterPersonJsonBuilder.Build();
        request.Age = -1;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Age)
            .WithErrorMessage(ResourceErrorMessages.PERSON_AGE_INVALID);
    }
}
