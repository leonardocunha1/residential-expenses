using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.Categories.Register;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace UseCases.Test.Categories.Register;

public class RegisterCategoryUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterCategoryJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Description.Should().Be(request.Description);
    }

    [Fact]
    public async Task Error_Description_Empty()
    {
        var request = RequestRegisterCategoryJsonBuilder.Build();
        request.Description = string.Empty;

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.CATEGORY_DESCRIPTION_EMPTY);
    }

    private static RegisterCategoryUseCase CreateUseCase()
    {
        var writeOnlyRepository = new CategoryWriteOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var mapper = MapperBuilder.Build();

        return new RegisterCategoryUseCase(writeOnlyRepository, unitOfWork, mapper);
    }
}
