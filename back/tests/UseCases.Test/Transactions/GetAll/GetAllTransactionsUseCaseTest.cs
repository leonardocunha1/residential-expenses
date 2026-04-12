using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.Transactions.GetAll;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Enums;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace UseCases.Test.Transactions.GetAll;

public class GetAllTransactionsUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var person = PersonBuilder.Build();

        var transactions = new List<Transaction>
        {
            new() { Id = 1, Description = "Compra", Value = 100, Type = TransactionType.Expense, CategoryId = 1, PersonId = person.Id },
            new() { Id = 2, Description = "Salário", Value = 3000, Type = TransactionType.Income, CategoryId = 2, PersonId = person.Id }
        };

        var useCase = CreateUseCase(loggedUser, person, transactions);

        var result = await useCase.Execute(person.Id);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Success_Empty_List()
    {
        var loggedUser = UserBuilder.Build();
        var person = PersonBuilder.Build();

        var useCase = CreateUseCase(loggedUser, person, []);

        var result = await useCase.Execute(person.Id);

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Error_Person_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        long invalidId = 999;

        var loggedUserService = new LoggedUserBuilder().Get(loggedUser).Build();
        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder()
            .GetByIdAndUserId(invalidId, loggedUser.Id, null)
            .Build();
        var transactionReadOnlyRepository = new TransactionReadOnlyRepositoryBuilder().Build();
        var mapper = MapperBuilder.Build();

        var useCase = new GetAllTransactionsUseCase(transactionReadOnlyRepository, personReadOnlyRepository, loggedUserService, mapper);

        Func<Task> act = async () => await useCase.Execute(invalidId);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    private static GetAllTransactionsUseCase CreateUseCase(User loggedUser, Person person, List<Transaction> transactions)
    {
        var loggedUserService = new LoggedUserBuilder().Get(loggedUser).Build();

        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder()
            .GetByIdAndUserId(person.Id, loggedUser.Id, person)
            .Build();

        var transactionReadOnlyRepository = new TransactionReadOnlyRepositoryBuilder()
            .GetAllByPersonId(person.Id, transactions)
            .Build();

        var mapper = MapperBuilder.Build();

        return new GetAllTransactionsUseCase(transactionReadOnlyRepository, personReadOnlyRepository, loggedUserService, mapper);
    }
}
