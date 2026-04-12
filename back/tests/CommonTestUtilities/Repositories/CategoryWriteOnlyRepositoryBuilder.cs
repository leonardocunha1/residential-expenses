using Moq;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Repositories.Category;

namespace CommonTestUtilities.Repositories;

public class CategoryWriteOnlyRepositoryBuilder
{
    private readonly Mock<ICategoryWriteOnlyRepository> _repository;

    public CategoryWriteOnlyRepositoryBuilder()
    {
        _repository = new Mock<ICategoryWriteOnlyRepository>();
    }

    public Mock<ICategoryWriteOnlyRepository> Mock => _repository;
    public ICategoryWriteOnlyRepository Build() => _repository.Object;
}
