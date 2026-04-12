using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.Application.UseCases.Transactions.GetAll;

public interface IGetAllTransactionsUseCase
{
    Task<List<ResponseShortTransactionJson>> Execute(long personId);
}
