using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.Application.UseCases.People.Register;

public interface IRegisterPersonUseCase
{
    Task<ResponseRegisteredPersonJson> Execute(RequestRegisterPersonJson request);
}
