using CommonTestUtilities.Entities;
using CommonTestUtilities.Mapper;
using CommonTestUtilities.Repositories;
using CommonTestUtilities.Requests;
using CommonTestUtilities.Services;
using FluentAssertions;
using ResidentialExpenses.Application.UseCases.Users.Register;
using ResidentialExpenses.Domain.Entities;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var useCase = CreateUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().NotBeNullOrWhiteSpace();
        result.Tokens.Should().NotBeNull();
        result.Tokens.AccessToken.Should().Be("generated-token");
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.NAME_EMPTY);
    }

    [Fact]
    public async Task Error_Email_Already_Registered()
    {
        var request = RequestRegisterUserJsonBuilder.Build();

        var existingUser = UserBuilder.Build();
        existingUser.Email = request.Email;

        var useCase = CreateUseCase(existingUser);

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED);
    }

    [Fact]
    public async Task Error_Password_Empty()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        var useCase = CreateUseCase();

        Func<Task> act = async () => await useCase.Execute(request);

        var exception = await act.Should().ThrowAsync<ErrorOnValidationException>();
        exception.Which.GetErrors().Should().Contain(ResourceErrorMessages.INVALID_PASSWORD);
    }

    private static RegisterUserUseCase CreateUseCase(User? existingUserByEmail = null)
    {
        var writeOnlyRepository = new UserWriteOnlyRepositoryBuilder().Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var unitOfWork = new UnitOfWorkBuilder().Build();
        var passwordEncripter = new PasswordEncripterBuilder().Build();
        var accessTokenGenerator = new AccessTokenGeneratorBuilder().Build();
        var mapper = MapperBuilder.Build();

        if (existingUserByEmail is not null)
            readOnlyRepository.GetUserByEmail(existingUserByEmail.Email, existingUserByEmail);

        return new RegisterUserUseCase(
            writeOnlyRepository,
            readOnlyRepository.Build(),
            unitOfWork,
            passwordEncripter,
            accessTokenGenerator,
            mapper);
    }
}
