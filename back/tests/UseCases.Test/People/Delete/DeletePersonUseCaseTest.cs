using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.People.Delete;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace UseCases.Test.People.Delete;

public class DeletePersonUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var person = PersonBuilder.Build();

        var useCase = CreateUseCase(loggedUser, person.Id, person);

        Func<Task> act = async () => await useCase.Execute(person.Id);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Person_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        long invalidId = 999;

        var useCase = CreateUseCase(loggedUser, invalidId, null);

        Func<Task> act = async () => await useCase.Execute(invalidId);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    private static DeletePersonUseCase CreateUseCase(User loggedUser, long personId, Person? person)
    {
        var loggedUserService = new LoggedUserBuilder().Get(loggedUser).Build();
        var readOnlyRepository = new PersonReadOnlyRepositoryBuilder()
            .GetByIdAndUserId(personId, loggedUser.Id, person)
            .Build();
        var writeOnlyRepository = new PersonWriteOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        return new DeletePersonUseCase(readOnlyRepository, writeOnlyRepository, loggedUserService, unitOfWork);
    }
}
