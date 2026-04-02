namespace ResidentialExpenses.Exceptions.ExceptionsBase;

public abstract class ResidentialExpensesException : SystemException
{
    protected ResidentialExpensesException(string message) : base(message)
    {
    }
    public abstract int StatusCode { get; }
    public abstract List<string> GetErrors();
}
