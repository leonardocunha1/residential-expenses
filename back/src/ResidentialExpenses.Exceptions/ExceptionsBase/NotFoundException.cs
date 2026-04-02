using System.Net;

namespace ResidentialExpenses.Exceptions.ExceptionsBase;

public class NotFoundException : ResidentialExpensesException
{
    public NotFoundException(string message) : base(message)
    { }
    public override int StatusCode => (int)HttpStatusCode.NotFound;
    public override List<string> GetErrors()
    {
        return [Message];
    }
}
