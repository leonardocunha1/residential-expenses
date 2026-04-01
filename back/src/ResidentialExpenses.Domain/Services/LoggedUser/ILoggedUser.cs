using ResidentialExpenses.Domain.Entities;

namespace ResidentialExpenses.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    Task<User> Get();
}

