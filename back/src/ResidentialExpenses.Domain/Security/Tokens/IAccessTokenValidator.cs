namespace ResidentialExpenses.Domain.Security.Tokens;

public interface IAccessTokenValidator
{
    long ValidateAndGetUserIdentifier(string token);
}
