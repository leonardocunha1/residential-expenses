namespace ResidentialExpenses.Application.UseCases.People.Delete;

public interface IDeletePersonUseCase
{
    Task Execute(long id);
}
