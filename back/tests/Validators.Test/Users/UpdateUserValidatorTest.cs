using CommonTestUtilities.Requests;
using FluentValidation.TestHelper;
using ResidentialExpenses.Application.UseCases.Users.Update;
using ResidentialExpenses.Exceptions;

namespace Validators.Test.Users;

public class UpdateUserValidatorTest
{
    private readonly UpdateUserValidator _validator = new();

    [Fact]
    public void Success_Without_Password_Change()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.OldPassword = string.Empty;
        request.NewPassword = string.Empty;

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Success_With_Password_Change()
    {
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_Name_Empty()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage(ResourceErrorMessages.NAME_EMPTY);
    }

    [Fact]
    public void Error_Email_Empty()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(ResourceErrorMessages.EMAIL_EMPTY);
    }

    [Fact]
    public void Error_Email_Invalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "email-invalido";

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage(ResourceErrorMessages.EMAIL_INVALID);
    }

    [Fact]
    public void Error_NewPassword_Invalid()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.NewPassword = "123";

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.NewPassword);
    }

    [Fact]
    public void Error_OldPassword_Empty_When_NewPassword_Provided()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.OldPassword = string.Empty;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.OldPassword)
            .WithErrorMessage(ResourceErrorMessages.INVALID_PASSWORD);
    }
}
