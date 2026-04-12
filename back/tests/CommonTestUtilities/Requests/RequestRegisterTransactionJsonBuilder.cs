using Bogus;
using ResidentialExpenses.Communication.Enums;
using ResidentialExpenses.Communication.Requests;

namespace CommonTestUtilities.Requests;

public class RequestRegisterTransactionJsonBuilder
{
    public static RequestRegisterTransactionJson Build()
    {
        return new Faker<RequestRegisterTransactionJson>()
            .RuleFor(t => t.Description, f => f.Commerce.ProductName())
            .RuleFor(t => t.Value, f => f.Finance.Amount(1, 10000))
            .RuleFor(t => t.Type, f => f.PickRandom<TransactionTypeJson>())
            .RuleFor(t => t.CategoryId, f => f.Random.Long(1, 1000))
            .RuleFor(t => t.PersonId, f => f.Random.Long(1, 1000))
            .Generate();
    }
}
