using CommonTestUtilities.Requests;
using FluentValidation.TestHelper;
using ResidentialExpenses.Application.UseCases.Transactions.Register;
using ResidentialExpenses.Communication.Enums;
using ResidentialExpenses.Exceptions;

namespace Validators.Test.Transactions;

public class RegisterTransactionValidatorTest
{
    private readonly RegisterTransactionValidator _validator = new();

    [Fact]
    public void Success()
    {
        var request = RequestRegisterTransactionJsonBuilder.Build();

        var result = _validator.TestValidate(request);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Error_Description_Empty()
    {
        var request = RequestRegisterTransactionJsonBuilder.Build();
        request.Description = string.Empty;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage(ResourceErrorMessages.TRANSACTION_DESCRIPTION_EMPTY);
    }

    [Fact]
    public void Error_Value_Zero()
    {
        var request = RequestRegisterTransactionJsonBuilder.Build();
        request.Value = 0;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Value)
            .WithErrorMessage(ResourceErrorMessages.TRANSACTION_VALUE_MUST_BE_POSITIVE);
    }

    [Fact]
    public void Error_Value_Negative()
    {
        var request = RequestRegisterTransactionJsonBuilder.Build();
        request.Value = -10;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Value)
            .WithErrorMessage(ResourceErrorMessages.TRANSACTION_VALUE_MUST_BE_POSITIVE);
    }

    [Fact]
    public void Error_Type_Invalid()
    {
        var request = RequestRegisterTransactionJsonBuilder.Build();
        request.Type = (TransactionTypeJson)999;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.Type)
            .WithErrorMessage(ResourceErrorMessages.TRANSACTION_TYPE_INVALID);
    }

    [Fact]
    public void Error_CategoryId_Zero()
    {
        var request = RequestRegisterTransactionJsonBuilder.Build();
        request.CategoryId = 0;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.CategoryId)
            .WithErrorMessage(ResourceErrorMessages.TRANSACTION_CATEGORY_NOT_FOUND);
    }

    [Fact]
    public void Error_PersonId_Zero()
    {
        var request = RequestRegisterTransactionJsonBuilder.Build();
        request.PersonId = 0;

        var result = _validator.TestValidate(request);

        result.ShouldHaveValidationErrorFor(x => x.PersonId)
            .WithErrorMessage(ResourceErrorMessages.PERSON_NOT_FOUND);
    }
}
