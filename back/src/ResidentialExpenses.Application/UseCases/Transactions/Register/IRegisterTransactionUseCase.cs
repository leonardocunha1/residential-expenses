using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.Application.UseCases.Transactions.Register;

public interface IRegisterTransactionUseCase
{
    Task<ResponseRegisteredTransactionJson> Execute(RequestRegisterTransactionJson request);
}
