namespace ResidentialExpenses.Communication.Responses;

public class ResponseCategoryTotalsJson
{
    public long CategoryId { get; set; }
    public string CategoryDescription { get; set; } = string.Empty;
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance { get; set; }
}
