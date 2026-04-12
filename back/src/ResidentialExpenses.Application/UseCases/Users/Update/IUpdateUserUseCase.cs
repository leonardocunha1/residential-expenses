using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.Application.UseCases.Users.Update;

public interface IUpdateUserUseCase
{
    Task<ResponseUserProfileJson> Execute(RequestUpdateUserJson request);
}
