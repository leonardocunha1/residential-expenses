using Moq;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }

    public UserReadOnlyRepositoryBuilder GetUserByEmail(string email, User? user)
    {
        _repository.Setup(r => r.GetUserByEmail(email)).ReturnsAsync(user);
        return this;
    }

    public UserReadOnlyRepositoryBuilder ExistActiveUserById(long id, bool exists)
    {
        _repository.Setup(r => r.ExistActiveUserById(id)).ReturnsAsync(exists);
        return this;
    }

    public IUserReadOnlyRepository Build() => _repository.Object;
}
