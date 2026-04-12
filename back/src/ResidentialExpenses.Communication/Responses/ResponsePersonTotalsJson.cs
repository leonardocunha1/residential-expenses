namespace ResidentialExpenses.Communication.Responses;

public class ResponsePersonTotalsJson
{
    public long PersonId { get; set; }
    public string PersonName { get; set; } = string.Empty;
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance { get; set; }
}
