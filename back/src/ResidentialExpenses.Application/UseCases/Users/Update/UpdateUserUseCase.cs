using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Communication.Responses;
using ResidentialExpenses.Domain.Repositories;
using ResidentialExpenses.Domain.Repositories.User;
using ResidentialExpenses.Domain.Security.Cryptography;
using ResidentialExpenses.Domain.Services.LoggedUser;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace ResidentialExpenses.Application.UseCases.Users.Update;

public class UpdateUserUseCase : IUpdateUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _updateOnlyRepository;
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordEncripter _passwordEncripter;

    public UpdateUserUseCase(
        ILoggedUser loggedUser,
        IUserUpdateOnlyRepository updateOnlyRepository,
        IUserReadOnlyRepository readOnlyRepository,
        IUnitOfWork unitOfWork,
        IPasswordEncripter passwordEncripter)
    {
        _loggedUser = loggedUser;
        _updateOnlyRepository = updateOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unitOfWork = unitOfWork;
        _passwordEncripter = passwordEncripter;
    }

    public async Task<ResponseUserProfileJson> Execute(RequestUpdateUserJson request)
    {
        var loggedUser = await _loggedUser.Get();

        await Validate(request, loggedUser.Email);

        var user = await _updateOnlyRepository.GetById(loggedUser.Id);

        user.Name = request.Name;
        user.Email = request.Email;

        if (string.IsNullOrWhiteSpace(request.NewPassword) == false)
        {
            var passwordMatch = _passwordEncripter.Verify(request.OldPassword, user.Password);

            if (passwordMatch == false)
                throw new ErrorOnValidationException([ResourceErrorMessages.OLD_PASSWORD_INCORRECT]);

            user.Password = _passwordEncripter.Encrypt(request.NewPassword);
        }

        _updateOnlyRepository.Update(user);

        await _unitOfWork.Commit();

        return new ResponseUserProfileJson
        {
            Name = user.Name,
            Email = user.Email
        };
    }

    private async Task Validate(RequestUpdateUserJson request, string currentEmail)
    {
        var validator = new UpdateUserValidator();

        var result = validator.Validate(request);

        if (request.Email.Equals(currentEmail) == false)
        {
            var emailExist = await _readOnlyRepository.GetUserByEmail(request.Email);
            if (emailExist != null)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceErrorMessages.EMAIL_ALREADY_REGISTERED));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
