using Bogus;
using DomainPerson = ResidentialExpenses.Domain.Entities.Person;

namespace CommonTestUtilities.Entities;

public class PersonBuilder
{
    public static DomainPerson Build(int? age = null)
    {
        return new Faker<DomainPerson>()
            .RuleFor(p => p.Id, f => f.Random.Long(1, 1000))
            .RuleFor(p => p.Name, f => f.Person.FullName)
            .RuleFor(p => p.Age, f => age ?? f.Random.Int(18, 80))
            .Generate();
    }
}
