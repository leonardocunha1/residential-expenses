namespace ResidentialExpenses.Domain.Repositories.Category;

public interface ICategoryReadOnlyRepository
{
    Task<List<Entities.Category>> GetAll();
    Task<Entities.Category?> GetById(long id);
}
