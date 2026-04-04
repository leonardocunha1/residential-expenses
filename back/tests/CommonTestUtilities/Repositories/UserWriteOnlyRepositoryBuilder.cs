using Moq;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Repositories.User;

namespace CommonTestUtilities.Repositories;

public class UserWriteOnlyRepositoryBuilder
{
    private readonly Mock<IUserWriteOnlyRepository> _repository;

    public UserWriteOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserWriteOnlyRepository>();
    }

    public Mock<IUserWriteOnlyRepository> Mock => _repository;
    public IUserWriteOnlyRepository Build() => _repository.Object;
}
