namespace ResidentialExpenses.Communication.Responses;

public class ResponseTotalsSummaryJson
{
    public List<ResponsePersonTotalsJson> People { get; set; } = [];
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance { get; set; }
}
