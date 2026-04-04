using Moq;
using ResidentialExpenses.Domain.Security.Cryptography;

namespace CommonTestUtilities.Services;

public class PasswordEncripterBuilder
{
    private readonly Mock<IPasswordEncripter> _encripter;

    public PasswordEncripterBuilder()
    {
        _encripter = new Mock<IPasswordEncripter>();

        _encripter.Setup(e => e.Encrypt(It.IsAny<string>())).Returns("encrypted-password");
    }

    public PasswordEncripterBuilder Verify(string password, bool result)
    {
        _encripter.Setup(e => e.Verify(password, It.IsAny<string>())).Returns(result);
        return this;
    }

    public IPasswordEncripter Build() => _encripter.Object;
}
