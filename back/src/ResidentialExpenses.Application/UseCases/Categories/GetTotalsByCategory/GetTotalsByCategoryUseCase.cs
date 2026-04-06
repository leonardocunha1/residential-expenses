using ResidentialExpenses.Communication.Responses;
using ResidentialExpenses.Domain.Enums;
using ResidentialExpenses.Domain.Repositories.Category;
using ResidentialExpenses.Domain.Repositories.Person;
using ResidentialExpenses.Domain.Repositories.Transaction;
using ResidentialExpenses.Domain.Services.LoggedUser;

namespace ResidentialExpenses.Application.UseCases.Categories.GetTotalsByCategory;

public class GetTotalsByCategoryUseCase : IGetTotalsByCategoryUseCase
{
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly ITransactionReadOnlyRepository _transactionReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;

    public GetTotalsByCategoryUseCase(
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        IPersonReadOnlyRepository personReadOnlyRepository,
        ITransactionReadOnlyRepository transactionReadOnlyRepository,
        ILoggedUser loggedUser)
    {
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _personReadOnlyRepository = personReadOnlyRepository;
        _transactionReadOnlyRepository = transactionReadOnlyRepository;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseCategoryTotalsSummaryJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var people = await _personReadOnlyRepository.GetAllByUserId(loggedUser.Id);
        var personIds = people.Select(p => p.Id).ToList();

        var transactions = await _transactionReadOnlyRepository.GetAllByPersonIds(personIds);

        var categories = await _categoryReadOnlyRepository.GetAll();

        var transactionsByCategory = transactions.GroupBy(t => t.CategoryId);

        var categoryTotals = categories.Select(category =>
        {
            var categoryTransactions = transactionsByCategory
                .FirstOrDefault(g => g.Key == category.Id);

            var totalIncome = categoryTransactions?
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Value) ?? 0;

            var totalExpense = categoryTransactions?
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Value) ?? 0;

            return new ResponseCategoryTotalsJson
            {
                CategoryId = category.Id,
                CategoryDescription = category.Description,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = totalIncome - totalExpense
            };
        }).ToList();

        return new ResponseCategoryTotalsSummaryJson
        {
            Categories = categoryTotals,
            TotalIncome = categoryTotals.Sum(c => c.TotalIncome),
            TotalExpense = categoryTotals.Sum(c => c.TotalExpense),
            Balance = categoryTotals.Sum(c => c.Balance)
        };
    }
}
