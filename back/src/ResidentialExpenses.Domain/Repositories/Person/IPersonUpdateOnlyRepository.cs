namespace ResidentialExpenses.Domain.Repositories.Person;

public interface IPersonUpdateOnlyRepository
{
    Task<Entities.Person?> GetByIdAndUserId(long id, long userId);
    void Update(Entities.Person person);
}
