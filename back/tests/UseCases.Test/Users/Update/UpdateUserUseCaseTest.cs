using CommonTestUtilities.Entities;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.Users.Update;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace UseCases.Test.Users.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.NewPassword = string.Empty;

        var useCase = CreateUseCase(loggedUser);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();

        loggedUser.Name.Should().Be(request.Name);
        loggedUser.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Success_With_Password_Change()
    {
        var oldPassword = "B2b@OldPass";
        var loggedUser = UserBuilder.Build();

        var request = RequestUpdateUserJsonBuilder.Build();
        request.OldPassword = oldPassword;

        var useCase = CreateUseCase(loggedUser, oldPassword);

        Func<Task> act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();

        loggedUser.Name.Should().Be(request.Name);
        loggedUser.Email.Should().Be(request.Email);
        loggedUser.Password.Should().Be("encrypted-password");
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;
        request.NewPassword = string.Empty;

        var useCase = CreateUseCase(loggedUser);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.NewPassword = string.Empty;

        var existingUser = UserBuilder.Build();
        existingUser.Email = request.Email;

        var useCase = CreateUseCase(loggedUser, existingUserByEmail: existingUser);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);
    }

    [Fact]
    public async Task Error_Old_Password_Incorrect()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.OldPassword = "B2b@WrongPass";

        var passwordEncripter = new PasswordEncripterBuilder()
            .Verify(request.OldPassword, false)
            .Build();

        var loggedUserService = new LoggedUserBuilder().Get(loggedUser).Build();
        var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(loggedUser).Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder().Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        var useCase = new UpdateUserUseCase(loggedUserService, updateOnlyRepository, readOnlyRepository, unitOfWork, passwordEncripter);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.OLD_PASSWORD_INCORRECT);
    }

    private static UpdateUserUseCase CreateUseCase(User loggedUser, string? verifyPassword = null, User? existingUserByEmail = null)
    {
        var loggedUserService = new LoggedUserBuilder().Get(loggedUser).Build();
        var updateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(loggedUser).Build();
        var unitOfWork = new UnitOfWorkBuilder().Build();

        var passwordEncripterBuilder = new PasswordEncripterBuilder();
        if (verifyPassword is not null)
            passwordEncripterBuilder.Verify(verifyPassword, true);
        var passwordEncripter = passwordEncripterBuilder.Build();

        var readOnlyRepositoryBuilder = new UserReadOnlyRepositoryBuilder();
        if (existingUserByEmail is not null)
            readOnlyRepositoryBuilder.GetUserByEmail(existingUserByEmail.Email, existingUserByEmail);
        var readOnlyRepository = readOnlyRepositoryBuilder.Build();

        return new UpdateUserUseCase(loggedUserService, updateOnlyRepository, readOnlyRepository, unitOfWork, passwordEncripter);
    }
}
