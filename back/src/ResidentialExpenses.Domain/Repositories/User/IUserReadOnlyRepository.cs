namespace ResidentialExpenses.Domain.Repositories.User;

public interface IUserReadOnlyRepository
{
    Task<Entities.User?> GetUserByEmail(string email);
    Task<bool> ExistActiveUserById(long id);
}