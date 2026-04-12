using Bogus;
using ResidentialExpenses.Communication.Enums;
using ResidentialExpenses.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterCategoryJsonBuilder
{
    public static RequestRegisterCategoryJson Build()
    {
        return new Faker<RequestRegisterCategoryJson>()
            .RuleFor(c => c.Description, f => f.Commerce.Categories(1)[0])
            .RuleFor(c => c.Purpose, f => f.PickRandom<CategoryPurposeJson>())
            .Generate();
    }
}
