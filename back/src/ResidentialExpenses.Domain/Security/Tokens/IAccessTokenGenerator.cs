using ResidentialExpenses.Domain.Entities;

namespace ResidentialExpenses.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}
