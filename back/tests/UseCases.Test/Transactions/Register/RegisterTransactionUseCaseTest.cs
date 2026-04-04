using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.Transactions.Register;
using ResidentialExpenses.Communication.Enums;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Enums;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace UseCases.Test.Transactions.Register;

public class RegisterTransactionUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var person = PersonBuilder.Build();
        var category = CategoryBuilder.Build(CategoryPurpose.Expense);

        var request = RequestRegisterTransactionJsonBuilder.Build();
        request.PersonId = person.Id;
        request.CategoryId = category.Id;
        request.Type = TransactionTypeJson.Expense;

        var useCase = CreateUseCase(loggedUser, person, category);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Description.Should().Be(request.Description);
        result.Value.Should().Be(request.Value);
    }

    [Fact]
    public async Task Success_Category_Both_Purpose()
    {
        var loggedUser = UserBuilder.Build();
        var person = PersonBuilder.Build();
        var category = CategoryBuilder.Build(CategoryPurpose.Both);

        var request = RequestRegisterTransactionJsonBuilder.Build();
        request.PersonId = person.Id;
        request.CategoryId = category.Id;
        request.Type = TransactionTypeJson.Income;

        var useCase = CreateUseCase(loggedUser, person, category);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
    }

    [Fact]
    public async Task Error_Description_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var person = PersonBuilder.Build();
        var category = CategoryBuilder.Build(CategoryPurpose.Expense);

        var request = RequestRegisterTransactionJsonBuilder.Build();
        request.Description = string.Empty;
        request.PersonId = person.Id;
        request.CategoryId = category.Id;
        request.Type = TransactionTypeJson.Expense;

        var useCase = CreateUseCase(loggedUser, person, category);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.TRANSACTION_DESCRIPTION_EMPTY);
    }

    [Fact]
    public async Task Error_Person_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var category = CategoryBuilder.Build(CategoryPurpose.Expense);
        long invalidPersonId = 999;

        var request = RequestRegisterTransactionJsonBuilder.Build();
        request.PersonId = invalidPersonId;
        request.CategoryId = category.Id;
        request.Type = TransactionTypeJson.Expense;

        var useCase = CreateUseCase(loggedUser, person: null, category, invalidPersonId);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.PERSON_NOT_FOUND);
    }

    [Fact]
    public async Task Error_Minor_Cannot_Have_Income()
    {
        var loggedUser = UserBuilder.Build();
        var minorPerson = PersonBuilder.Build(age: 15);
        var category = CategoryBuilder.Build(CategoryPurpose.Income);

        var request = RequestRegisterTransactionJsonBuilder.Build();
        request.PersonId = minorPerson.Id;
        request.CategoryId = category.Id;
        request.Type = TransactionTypeJson.Income;

        var useCase = CreateUseCase(loggedUser, minorPerson, category);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.TRANSACTION_MINOR_ONLY_EXPENSE);
    }

    [Fact]
    public async Task Error_Category_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var person = PersonBuilder.Build();
        long invalidCategoryId = 999;

        var request = RequestRegisterTransactionJsonBuilder.Build();
        request.PersonId = person.Id;
        request.CategoryId = invalidCategoryId;
        request.Type = TransactionTypeJson.Expense;

        var useCase = CreateUseCase(loggedUser, person, category: null, invalidCategoryId: invalidCategoryId);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.TRANSACTION_CATEGORY_NOT_FOUND);
    }

    [Fact]
    public async Task Error_Category_Purpose_Mismatch()
    {
        var loggedUser = UserBuilder.Build();
        var person = PersonBuilder.Build();
        var category = CategoryBuilder.Build(CategoryPurpose.Expense);

        var request = RequestRegisterTransactionJsonBuilder.Build();
        request.PersonId = person.Id;
        request.CategoryId = category.Id;
        request.Type = TransactionTypeJson.Income;

        var useCase = CreateUseCase(loggedUser, person, category);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.TRANSACTION_CATEGORY_PURPOSE_MISMATCH);
    }

    private static RegisterTransactionUseCase CreateUseCase(
        User loggedUser,
        Person? person,
        Category? category,
        long? invalidPersonId = null,
        long? invalidCategoryId = null)
    {
        var loggedUserService = new LoggedUserBuilder().Get(loggedUser).Build();
        var writeOnlyRepository = new TransactionWriteOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var mapper = MapperBuilder.Build();

        var personReadOnlyRepositoryBuilder = new PersonReadOnlyRepositoryBuilder();
        if (person is not null)
            personReadOnlyRepositoryBuilder.GetByIdAndUserId(person.Id, loggedUser.Id, person);
        else
            personReadOnlyRepositoryBuilder.GetByIdAndUserId(invalidPersonId ?? 999, loggedUser.Id, null);

        var categoryReadOnlyRepositoryBuilder = new CategoryReadOnlyRepositoryBuilder();
        if (category is not null)
            categoryReadOnlyRepositoryBuilder.GetById(category.Id, category);
        else
            categoryReadOnlyRepositoryBuilder.GetById(invalidCategoryId ?? 999, null);

        return new RegisterTransactionUseCase(
            writeOnlyRepository,
            personReadOnlyRepositoryBuilder.Build(),
            categoryReadOnlyRepositoryBuilder.Build(),
            loggedUserService,
            unitOfWork,
            mapper);
    }
}
