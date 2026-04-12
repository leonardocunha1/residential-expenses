using Moq;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Services.LoggedUser;

namespace CommonTestUtilities.Services;

public class LoggedUserBuilder
{
    private readonly Mock<ILoggedUser> _loggedUser;

    public LoggedUserBuilder()
    {
        _loggedUser = new Mock<ILoggedUser>();
    }

    public LoggedUserBuilder Get(User user)
    {
        _loggedUser.Setup(l => l.Get()).ReturnsAsync(user);
        return this;
    }

    public ILoggedUser Build() => _loggedUser.Object;
}
