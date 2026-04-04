using Bogus;
using ResidentialExpenses.Domain.Entities;

namespace CommonTestUtilities.Entities;

public class UserBuilder
{
    public static User Build()
    {
        return new Faker<User>()
            .RuleFor(u => u.Id, f => f.Random.Long(1, 1000))
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, _ => "hashed-password")
            .RuleFor(u => u.CreatedAt, f => f.Date.Past())
            .Generate();
    }
}
