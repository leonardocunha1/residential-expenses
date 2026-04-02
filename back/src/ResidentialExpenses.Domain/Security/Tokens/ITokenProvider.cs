namespace ResidentialExpenses.Domain.Security.Tokens;

public interface ITokenProvider
{
    public string TokenOnRequest();
}