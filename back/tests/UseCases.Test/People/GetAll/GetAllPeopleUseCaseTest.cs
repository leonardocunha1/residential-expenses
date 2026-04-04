using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.People.GetAll;
using ResidentialExpenses.Domain.Entities;

namespace UseCases.Test.People.GetAll;

public class GetAllPeopleUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();

        var people = new List<Person>
        {
            PersonBuilder.Build(),
            PersonBuilder.Build()
        };

        var useCase = CreateUseCase(loggedUser, people);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task Success_Empty_List()
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(loggedUser, []);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    private static GetAllPeopleUseCase CreateUseCase(User loggedUser, List<Person> people)
    {
        var loggedUserService = new LoggedUserBuilder().Get(loggedUser).Build();
        var readOnlyRepository = new PersonReadOnlyRepositoryBuilder()
            .GetAllByUserId(loggedUser.Id, people)
            .Build();
        var mapper = MapperBuilder.Build();

        return new GetAllPeopleUseCase(readOnlyRepository, loggedUserService, mapper);
    }
}
