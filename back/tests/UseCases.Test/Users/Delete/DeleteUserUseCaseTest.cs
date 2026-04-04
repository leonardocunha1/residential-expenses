using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.Users.Delete;

namespace UseCases.Test.Users.Delete;

public class DeleteUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();

        var loggedUserService = new LoggedUserBuilder().Get(loggedUser).Build();
        var writeOnlyRepository = new UserWriteOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        var useCase = new DeleteUserUseCase(loggedUserService, writeOnlyRepository, unitOfWork);

        Func<Task> act = async () => await useCase.Execute();

        await act.Should().NotThrowAsync();
    }
}
