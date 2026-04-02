using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.Application.UseCases.Categories.GetAll;

public interface IGetAllCategoriesUseCase
{
    Task<List<ResponseShortCategoryJson>> Execute();
}
