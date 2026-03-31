namespace ResidentialExpenses.Domain.Entities;

public class UserPerson
{
    public long UserId { get; set; }
    public long PersonId { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Person Person { get; set; } = null!;
}