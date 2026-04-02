using System.Net;

namespace ResidentialExpenses.Exceptions.ExceptionsBase;

public class InvalidLoginException : ResidentialExpensesException
{
    public InvalidLoginException() : base(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID)
    {
    }

    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors()
    {
        return [Message];
    }
}