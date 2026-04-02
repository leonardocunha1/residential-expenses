using System.Net;

namespace ResidentialExpenses.Exceptions.ExceptionsBase;

public class ResidentialExpensesUnauthorizedException : ResidentialExpensesException
{
    public ResidentialExpensesUnauthorizedException(string message) : base(message)
    {
    }

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}
