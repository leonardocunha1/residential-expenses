using Bogus;
using ResidentialExpenses.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(u => u.Name, f => f.Person.FullName)
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.OldPassword, f => f.Internet.Password(prefix: "B2b@"))
            .RuleFor(u => u.NewPassword, f => f.Internet.Password(prefix: "C3c!"))
            .Generate();
    }
}
