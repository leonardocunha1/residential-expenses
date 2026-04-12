using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.Application.UseCases.Transactions.GetTotalsByPerson;

public interface IGetTotalsByPersonUseCase
{
    Task<ResponseTotalsSummaryJson> Execute();
}
