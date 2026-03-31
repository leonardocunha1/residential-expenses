namespace ResidentialExpenses.Domain.Entities;

public class Person
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty; // max 200
    public int Age { get; set; }

    // Navigation properties
    public ICollection<UserPerson> UserPeople { get; set; } = [];
    public ICollection<Transaction> Transactions { get; set; } = [];
}