using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.Transactions.GetTotalsByPerson;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Enums;

namespace UseCases.Test.Transactions.GetTotalsByPerson;

public class GetTotalsByPersonUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var personA = PersonBuilder.Build();
        var personB = PersonBuilder.Build();
        var people = new List<Person> { personA, personB };

        var transactions = new List<Transaction>
        {
            new() { Id = 1, Value = 500, Type = TransactionType.Expense, CategoryId = 1, PersonId = personA.Id },
            new() { Id = 2, Value = 3000, Type = TransactionType.Income, CategoryId = 2, PersonId = personA.Id },
            new() { Id = 3, Value = 200, Type = TransactionType.Expense, CategoryId = 1, PersonId = personB.Id },
            new() { Id = 4, Value = 1000, Type = TransactionType.Income, CategoryId = 2, PersonId = personB.Id }
        };

        var personIds = people.Select(p => p.Id).ToList();
        var useCase = CreateUseCase(loggedUser, people, personIds, transactions);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.People.Should().HaveCount(2);
        result.TotalIncome.Should().Be(4000);
        result.TotalExpense.Should().Be(700);
        result.Balance.Should().Be(3300);

        var resultA = result.People.First(p => p.PersonId == personA.Id);
        resultA.PersonName.Should().Be(personA.Name);
        resultA.TotalIncome.Should().Be(3000);
        resultA.TotalExpense.Should().Be(500);
        resultA.Balance.Should().Be(2500);

        var resultB = result.People.First(p => p.PersonId == personB.Id);
        resultB.PersonName.Should().Be(personB.Name);
        resultB.TotalIncome.Should().Be(1000);
        resultB.TotalExpense.Should().Be(200);
        resultB.Balance.Should().Be(800);
    }

    [Fact]
    public async Task Success_No_Transactions()
    {
        var loggedUser = UserBuilder.Build();
        var person = PersonBuilder.Build();

        var useCase = CreateUseCase(loggedUser, [person], [person.Id], []);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.People.Should().ContainSingle();
        result.TotalIncome.Should().Be(0);
        result.TotalExpense.Should().Be(0);
        result.Balance.Should().Be(0);
    }

    [Fact]
    public async Task Success_No_People()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser, [], [], []);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.People.Should().BeEmpty();
        result.TotalIncome.Should().Be(0);
        result.TotalExpense.Should().Be(0);
        result.Balance.Should().Be(0);
    }

    private static GetTotalsByPersonUseCase CreateUseCase(
        User loggedUser,
        List<Person> people,
        List<long> personIds,
        List<Transaction> transactions)
    {
        var loggedUserService = new LoggedUserBuilder().Get(loggedUser).Build();

        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder()
            .GetAllByUserId(loggedUser.Id, people)
            .Build();

        var transactionReadOnlyRepository = new TransactionReadOnlyRepositoryBuilder()
            .GetAllByPersonIds(personIds, transactions)
            .Build();

        return new GetTotalsByPersonUseCase(personReadOnlyRepository, transactionReadOnlyRepository, loggedUserService);
    }
}
