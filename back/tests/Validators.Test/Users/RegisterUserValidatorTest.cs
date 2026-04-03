using CommonTestUtilities.Requests;
using FluentValidation.TestHelper;
using ResidentialExpenses.Application.UseCases.Users.Register;
using ResidentialExpenses.Exceptions;

namespace Validators.Test.Users;

public class RegisterUserValidatorTest
{
    private readonly RegisterUserValidator _validator = new();

    [Fact]
    public void Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(ResourceErrorMessages.NAME_EMPTY);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(ResourceErrorMessages.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Email = "email-invalido";

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(ResourceErrorMessages.EMAIL_INVALID);
    }

    [Fact]
    public void Error_Password_Invalid()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = "123";

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}
