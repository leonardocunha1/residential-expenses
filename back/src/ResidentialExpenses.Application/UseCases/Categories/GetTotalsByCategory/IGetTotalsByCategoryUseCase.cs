using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.Application.UseCases.Categories.GetTotalsByCategory;

public interface IGetTotalsByCategoryUseCase
{
    Task<ResponseCategoryTotalsSummaryJson> Execute();
}
