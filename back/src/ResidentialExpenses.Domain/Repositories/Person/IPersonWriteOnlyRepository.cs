namespace ResidentialExpenses.Domain.Repositories.Person;

public interface IPersonWriteOnlyRepository
{
    Task Add(Entities.Person person);
    Task Delete(Entities.Person person);
}
