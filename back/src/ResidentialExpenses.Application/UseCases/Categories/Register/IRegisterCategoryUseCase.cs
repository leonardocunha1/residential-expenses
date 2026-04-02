using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.Application.UseCases.Categories.Register;

public interface IRegisterCategoryUseCase
{
    Task<ResponseRegisteredCategoryJson> Execute(RequestRegisterCategoryJson request);
}
