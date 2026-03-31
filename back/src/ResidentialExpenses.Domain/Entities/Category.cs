using ResidentialExpenses.Domain.Enums;

namespace ResidentialExpenses.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;   // max 400
    public CategoryPurpose Purpose { get; set; }

    // Navigation properties
    public ICollection<Transaction> Transactions { get; set; } = [];
}