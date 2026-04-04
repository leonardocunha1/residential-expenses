using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Services;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.Users.Login;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace UseCases.Test.Login;

public class DoLoginUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var password = "A1a!validPass";
        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user, password);

        var request = new RequestLoginJson
        {
            Email = user.Email,
            Password = password
        };

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Tokens.AccessToken.Should().Be("generated-token");
    }

    [Fact]
    public async Task Error_User_Not_Found()
    {
        var useCase = CreateUseCase(user: null, password: "any");

        var request = new RequestLoginJson
        {
            Email = "notfound@test.com",
            Password = "A1a!validPass"
        };

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<InvalidLoginException>();
    }

    [Fact]
    public async Task Error_Password_Wrong()
    {
        var user = UserBuilder.Build();

        var passwordEncripter = new PasswordEncripterBuilder()
            .Verify("wrong-password", false)
            .Build();

        var readOnlyRepository = new UserReadOnlyRepositoryBuilder()
            .GetUserByEmail(user.Email, user)
            .Build();

        var accessTokenGenerator = new AccessTokenGeneratorBuilder().Build();

        var useCase = new DoLoginUseCase(readOnlyRepository, passwordEncripter, accessTokenGenerator);

        var request = new RequestLoginJson
        {
            Email = user.Email,
            Password = "wrong-password"
        };

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().ThrowAsync<InvalidLoginException>();
    }

    private static DoLoginUseCase CreateUseCase(User? user, string password)
    {
        var passwordEncripter = new PasswordEncripterBuilder()
            .Verify(password, true)
            .Build();

        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();

        if (user is not null)
            readOnlyRepository.GetUserByEmail(user.Email, user);

        var accessTokenGenerator = new AccessTokenGeneratorBuilder().Build();

        return new DoLoginUseCase(readOnlyRepository.Build(), passwordEncripter, accessTokenGenerator);
    }
}
