using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.People.Update;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace UseCases.Test.People.Update;

public class UpdatePersonUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var person = PersonBuilder.Build();

        var request = RequestUpdatePersonJsonBuilder.Build();

        var useCase = CreateUseCase(loggedUser, person);

        Func<Task> act = async () => await useCase.Execute(person.Id, request);

        await act.Should().NotThrowAsync();

        person.Name.Should().Be(request.Name);
        person.Age.Should().Be(request.Age);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var person = PersonBuilder.Build();

        var request = RequestUpdatePersonJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(loggedUser, person);

        Func<Task> act = async () => await useCase.Execute(person.Id, request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.PERSON_NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Person_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestUpdatePersonJsonBuilder.Build();
        long invalidId = 999;

        var useCase = CreateUseCase(loggedUser, person: null, invalidId);

        Func<Task> act = async () => await useCase.Execute(invalidId, request);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    private static UpdatePersonUseCase CreateUseCase(User loggedUser, Person? person, long? personId = null)
    {
        var loggedUserService = new LoggedUserBuilder().Get(loggedUser).Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        var id = personId ?? person!.Id;

        var updateOnlyRepositoryBuilder = new PersonUpdateOnlyRepositoryBuilder()
            .GetByIdAndUserId(id, loggedUser.Id, person);

        return new UpdatePersonUseCase(updateOnlyRepositoryBuilder.Build(), loggedUserService, unitOfWork);
    }
}
