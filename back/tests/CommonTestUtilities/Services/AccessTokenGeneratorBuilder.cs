using Moq;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Security.Tokens;

namespace CommonTestUtilities.Services;

public class AccessTokenGeneratorBuilder
{
    private readonly Mock<IAccessTokenGenerator> _generator;

    public AccessTokenGeneratorBuilder()
    {
        _generator = new Mock<IAccessTokenGenerator>();

        _generator.Setup(g => g.Generate(It.IsAny<User>())).Returns("generated-token");
    }

    public IAccessTokenGenerator Build() => _generator.Object;
}
