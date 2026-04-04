using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.Categories.GetTotalsByCategory;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Enums;

namespace UseCases.Test.Categories.GetTotalsByCategory;

public class GetTotalsByCategoryUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var personA = PersonBuilder.Build();
        var personB = PersonBuilder.Build();
        var people = new List<Person> { personA, personB };

        var catExpense = CategoryBuilder.Build(CategoryPurpose.Expense);
        var catIncome = CategoryBuilder.Build(CategoryPurpose.Income);
        var categories = new List<Category> { catExpense, catIncome };

        var transactions = new List<Transaction>
        {
            new() { Id = 1, Value = 500, Type = TransactionType.Expense, CategoryId = catExpense.Id, PersonId = personA.Id },
            new() { Id = 2, Value = 300, Type = TransactionType.Expense, CategoryId = catExpense.Id, PersonId = personB.Id },
            new() { Id = 3, Value = 3000, Type = TransactionType.Income, CategoryId = catIncome.Id, PersonId = personA.Id }
        };

        var personIds = people.Select(p => p.Id).ToList();
        var useCase = CreateUseCase(loggedUser, people, personIds, transactions, categories);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Categories.Should().HaveCount(2);
        result.TotalIncome.Should().Be(3000);
        result.TotalExpense.Should().Be(800);
        result.Balance.Should().Be(2200);

        var expenseResult = result.Categories.First(c => c.CategoryId == catExpense.Id);
        expenseResult.TotalExpense.Should().Be(800);
        expenseResult.TotalIncome.Should().Be(0);
        expenseResult.Balance.Should().Be(-800);

        var incomeResult = result.Categories.First(c => c.CategoryId == catIncome.Id);
        incomeResult.TotalIncome.Should().Be(3000);
        incomeResult.TotalExpense.Should().Be(0);
        incomeResult.Balance.Should().Be(3000);
    }

    [Fact]
    public async Task Success_No_Transactions()
    {
        var loggedUser = UserBuilder.Build();
        var person = PersonBuilder.Build();
        var category = CategoryBuilder.Build(CategoryPurpose.Expense);

        var useCase = CreateUseCase(loggedUser, [person], [person.Id], [], [category]);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.TotalIncome.Should().Be(0);
        result.TotalExpense.Should().Be(0);
        result.Balance.Should().Be(0);
    }

    private static GetTotalsByCategoryUseCase CreateUseCase(
        User loggedUser,
        List<Person> people,
        List<long> personIds,
        List<Transaction> transactions,
        List<Category> categories)
    {
        var loggedUserService = new LoggedUserBuilder().Get(loggedUser).Build();

        var personReadOnlyRepository = new PersonReadOnlyRepositoryBuilder()
            .GetAllByUserId(loggedUser.Id, people)
            .Build();

        var transactionReadOnlyRepository = new TransactionReadOnlyRepositoryBuilder()
            .GetAllByPersonIds(personIds, transactions)
            .Build();

        var categoryReadOnlyRepository = new CategoryReadOnlyRepositoryBuilder()
            .GetAll(categories)
            .Build();

        return new GetTotalsByCategoryUseCase(
            categoryReadOnlyRepository,
            personReadOnlyRepository,
            transactionReadOnlyRepository,
            loggedUserService);
    }
}
