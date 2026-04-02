using ResidentialExpenses.Communication.Requests;

namespace ResidentialExpenses.Application.UseCases.Users.Update;

public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
