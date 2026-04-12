namespace ResidentialExpenses.Communication.Responses;

public class ResponseCategoryTotalsSummaryJson
{
    public List<ResponseCategoryTotalsJson> Categories { get; set; } = [];
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance { get; set; }
}
