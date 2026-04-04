using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.Categories.GetAll;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Domain.Enums;

namespace UseCases.Test.Categories.GetAll;

public class GetAllCategoriesUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var categories = new List<Category>
        {
            CategoryBuilder.Build(CategoryPurpose.Expense),
            CategoryBuilder.Build(CategoryPurpose.Income)
        };

        var useCase = CreateUseCase(categories);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Success_Empty_List()
    {
        var useCase = CreateUseCase([]);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    private static GetAllCategoriesUseCase CreateUseCase(List<Category> categories)
    {
        var readOnlyRepository = new CategoryReadOnlyRepositoryBuilder()
            .GetAll(categories)
            .Build();
        var mapper = MapperBuilder.Build();

        return new GetAllCategoriesUseCase(readOnlyRepository, mapper);
    }
}
