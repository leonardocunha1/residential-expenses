using ResidentialExpenses.Domain.Enums;

namespace ResidentialExpenses.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;   // max 400
    public decimal Value { get; set; }
    public TransactionType Type { get; set; }

    public Guid CategoryId { get; set; }
    public Guid PersonId { get; set; }

    // Navigation properties
    public Category Category { get; set; } = null!;
    public Person Person { get; set; } = null!;
}