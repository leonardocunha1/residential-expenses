using Moq;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Repositories.Person;

namespace CommonTestUtilities.Repositories;

public class PersonReadOnlyRepositoryBuilder
{
    private readonly Mock<IPersonReadOnlyRepository> _repository;

    public PersonReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IPersonReadOnlyRepository>();
    }

    public PersonReadOnlyRepositoryBuilder GetAllByUserId(long userId, List<Person> people)
    {
        _repository.Setup(r => r.GetAllByUserId(userId)).ReturnsAsync(people);
        return this;
    }

    public PersonReadOnlyRepositoryBuilder GetByIdAndUserId(long id, long userId, Person? person)
    {
        _repository.Setup(r => r.GetByIdAndUserId(id, userId)).ReturnsAsync(person);
        return this;
    }

    public IPersonReadOnlyRepository Build() => _repository.Object;
}
