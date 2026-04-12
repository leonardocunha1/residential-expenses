using Bogus;
using ResidentialExpenses.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestUpdatePersonJsonBuilder
{
    public static RequestUpdatePersonJson Build()
    {
        return new Faker<RequestUpdatePersonJson>()
            .RuleFor(p => p.Name, f => f.Person.FullName)
            .RuleFor(p => p.Age, f => f.Random.Int(1, 100))
            .Generate();
    }
}
