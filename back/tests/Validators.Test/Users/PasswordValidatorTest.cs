using CommonTestUtilities.Requests;
using FluentValidation;
using FluentValidation.TestHelper;
using ResidentialExpenses.Application.SharedValidators;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Exceptions;

namespace Validators.Test.Users;

public class PasswordValidatorTest
{
    private readonly InlineValidator<RequestRegisterUserJson> _validator;

    public PasswordValidatorTest()
    {
        _validator = new InlineValidator<RequestRegisterUserJson>();
        _validator.RuleFor(x => x.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
    }

    [Fact]
    public void Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveValidationErrorFor(x => x.Password);
    }

    [Theory]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("AAAAAAAA")]
    [InlineData("Aaaaaaaa")]
    [InlineData("Aaaaaaa1")]
    public void Error_Invalid_Password(string password)
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = password;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage(ResourceErrorMessages.INVALID_PASSWORD);
    }
}
