using ResidentialExpenses.Communication.Requests;

namespace ResidentialExpenses.Application.UseCases.People.Update;

public interface IUpdatePersonUseCase
{
    Task Execute(long id, RequestUpdatePersonJson request);
}
