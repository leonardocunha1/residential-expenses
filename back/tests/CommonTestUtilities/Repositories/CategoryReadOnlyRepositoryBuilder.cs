using Moq;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Repositories.Category;

namespace CommonTestUtilities.Repositories;

public class CategoryReadOnlyRepositoryBuilder
{
    private readonly Mock<ICategoryReadOnlyRepository> _repository;

    public CategoryReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<ICategoryReadOnlyRepository>();
    }

    public CategoryReadOnlyRepositoryBuilder GetAll(List<Category> categories)
    {
        _repository.Setup(r => r.GetAll()).ReturnsAsync(categories);
        return this;
    }

    public CategoryReadOnlyRepositoryBuilder GetById(long id, Category? category)
    {
        _repository.Setup(r => r.GetById(id)).ReturnsAsync(category);
        return this;
    }

    public ICategoryReadOnlyRepository Build() => _repository.Object;
}
