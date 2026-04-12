using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.People.Register;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace UseCases.Test.People.Register;

public class RegisterPersonUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterPersonJsonBuilder.Build();
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Age.Should().Be(request.Age);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterPersonJsonBuilder.Build();
        request.Name = string.Empty;

        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.PERSON_NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Age_Invalid()
    {
        var request = RequestRegisterPersonJsonBuilder.Build();
        request.Age = 0;

        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.PERSON_AGE_INVALID);
    }

    private static RegisterPersonUseCase CreateUseCase(User loggedUser)
    {
        var writeOnlyRepository = new PersonWriteOnlyRepositoryBuilder().Build();
        var userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(loggedUser).Build();
        var loggedUserService = new LoggedUserBuilder().Get(loggedUser).Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var mapper = MapperBuilder.Build();

        return new RegisterPersonUseCase(
            writeOnlyRepository,
            userUpdateOnlyRepository,
            loggedUserService,
            unitOfWork,
            mapper);
    }
}
