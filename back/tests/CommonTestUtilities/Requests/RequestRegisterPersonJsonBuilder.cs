using Bogus;
using ResidentialExpenses.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterPersonJsonBuilder
{
    public static RequestRegisterPersonJson Build()
    {
        return new Faker<RequestRegisterPersonJson>()
            .RuleFor(p => p.Name, f => f.Person.FullName)
            .RuleFor(p => p.Age, f => f.Random.Int(1, 100))
            .Generate();
    }
}
