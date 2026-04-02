namespace ResidentialExpenses.Domain.Repositories.Person;

public interface IPersonReadOnlyRepository
{
    Task<List<Entities.Person>> GetAllByUserId(long userId);
    Task<Entities.Person?> GetByIdAndUserId(long id, long userId);
}
