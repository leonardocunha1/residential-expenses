using ResidentialExpenses.Communication.Responses;
using ResidentialExpenses.Domain.Enums;
using ResidentialExpenses.Domain.Repositories.Person;
using ResidentialExpenses.Domain.Repositories.Transaction;
using ResidentialExpenses.Domain.Services.LoggedUser;

namespace ResidentialExpenses.Application.UseCases.Transactions.GetTotalsByPerson;

public class GetTotalsByPersonUseCase : IGetTotalsByPersonUseCase
{
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly ITransactionReadOnlyRepository _transactionReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;

    public GetTotalsByPersonUseCase(
        IPersonReadOnlyRepository personReadOnlyRepository,
        ITransactionReadOnlyRepository transactionReadOnlyRepository,
        ILoggedUser loggedUser)
    {
        _personReadOnlyRepository = personReadOnlyRepository;
        _transactionReadOnlyRepository = transactionReadOnlyRepository;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseTotalsSummaryJson> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var people = await _personReadOnlyRepository.GetAllByUserId(loggedUser.Id);
        var personIds = people.Select(p => p.Id).ToList();

        var transactions = await _transactionReadOnlyRepository.GetAllByPersonIds(personIds);

        var transactionsByPerson = transactions.GroupBy(t => t.PersonId);

        var peopleTotals = people.Select(person =>
        {
            var personTransactions = transactionsByPerson
                .FirstOrDefault(g => g.Key == person.Id);

            var totalIncome = personTransactions?
                .Where(t => t.Type == TransactionType.Income)
                .Sum(t => t.Value) ?? 0;

            var totalExpense = personTransactions?
                .Where(t => t.Type == TransactionType.Expense)
                .Sum(t => t.Value) ?? 0;

            return new ResponsePersonTotalsJson
            {
                PersonId = person.Id,
                PersonName = person.Name,
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = totalIncome - totalExpense
            };
        }).ToList();

        return new ResponseTotalsSummaryJson
        {
            People = peopleTotals,
            TotalIncome = peopleTotals.Sum(p => p.TotalIncome),
            TotalExpense = peopleTotals.Sum(p => p.TotalExpense),
            Balance = peopleTotals.Sum(p => p.Balance)
        };
    }
}
