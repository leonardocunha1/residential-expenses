using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.Application.UseCases.People.GetAll;

public interface IGetAllPeopleUseCase
{
    Task<List<ResponseShortPersonJson>> Execute();
}
