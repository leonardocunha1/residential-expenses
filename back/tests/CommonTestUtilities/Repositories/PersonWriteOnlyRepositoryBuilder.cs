using Moq;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Repositories.Person;

namespace CommonTestUtilities.Repositories;

public class PersonWriteOnlyRepositoryBuilder
{
    private readonly Mock<IPersonWriteOnlyRepository> _repository;

    public PersonWriteOnlyRepositoryBuilder()
    {
        _repository = new Mock<IPersonWriteOnlyRepository>();
    }

    public Mock<IPersonWriteOnlyRepository> Mock => _repository;
    public IPersonWriteOnlyRepository Build() => _repository.Object;
}
