using Moq;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Repositories.Person;

namespace CommonTestUtilities.Repositories;

public class PersonUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IPersonUpdateOnlyRepository> _repository;

    public PersonUpdateOnlyRepositoryBuilder()
    {
        _repository = new Mock<IPersonUpdateOnlyRepository>();
    }

    public PersonUpdateOnlyRepositoryBuilder GetByIdAndUserId(long id, long userId, Person? person)
    {
        _repository.Setup(r => r.GetByIdAndUserId(id, userId)).ReturnsAsync(person);
        return this;
    }

    public IPersonUpdateOnlyRepository Build() => _repository.Object;
}
