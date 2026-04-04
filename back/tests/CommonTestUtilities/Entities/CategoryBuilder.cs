using Bogus;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Enums;

namespace CommonTestUtilities.Entities;

public class CategoryBuilder
{
    public static Category Build(CategoryPurpose? purpose = null)
    {
        return new Faker<Category>()
            .RuleFor(c => c.Id, f => f.Random.Long(1, 1000))
            .RuleFor(c => c.Description, f => f.Commerce.Categories(1)[0])
            .RuleFor(c => c.Purpose, f => purpose ?? f.PickRandom<CategoryPurpose>())
            .Generate();
    }
}
