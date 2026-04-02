using ResidentialExpenses.Communication.Enums;

namespace ResidentialExpenses.Communication.Responses;

public class ResponseShortTransactionJson
{
    public long Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public TransactionTypeJson Type { get; set; }
    public long CategoryId { get; set; }
    public long PersonId { get; set; }
}
