using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Domain.Repositories;
using ResidentialExpenses.Domain.Repositories.Person;
using ResidentialExpenses.Domain.Services.LoggedUser;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace ResidentialExpenses.Application.UseCases.People.Update;

public class UpdatePersonUseCase : IUpdatePersonUseCase
{
    private readonly IPersonUpdateOnlyRepository _updateOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePersonUseCase(
        IPersonUpdateOnlyRepository updateOnlyRepository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork)
    {
        _updateOnlyRepository = updateOnlyRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long id, RequestUpdatePersonJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var person = await _updateOnlyRepository.GetByIdAndUserId(id, loggedUser.Id);

        if (person is null)
            throw new NotFoundException(ResourceErrorMessages.PERSON_NOT_FOUND);

        person.Name = request.Name;
        person.Age = request.Age;

        _updateOnlyRepository.Update(person);

        await _unitOfWork.Commit();
    }

    private static void Validate(RequestUpdatePersonJson request)
    {
        var validator = new UpdatePersonValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
