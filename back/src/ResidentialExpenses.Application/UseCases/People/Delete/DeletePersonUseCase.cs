using ResidentialExpenses.Domain.Repositories;
using ResidentialExpenses.Domain.Repositories.Person;
using ResidentialExpenses.Domain.Services.LoggedUser;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace ResidentialExpenses.Application.UseCases.People.Delete;

public class DeletePersonUseCase : IDeletePersonUseCase
{
    private readonly IPersonReadOnlyRepository _readOnlyRepository;
    private readonly IPersonWriteOnlyRepository _writeOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePersonUseCase(
        IPersonReadOnlyRepository readOnlyRepository,
        IPersonWriteOnlyRepository writeOnlyRepository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork)
    {
        _readOnlyRepository = readOnlyRepository;
        _writeOnlyRepository = writeOnlyRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var person = await _readOnlyRepository.GetByIdAndUserId(id, loggedUser.Id);

        if (person is null)
            throw new NotFoundException(ResourceErrorMessages.PERSON_NOT_FOUND);

        await _writeOnlyRepository.Delete(person);

        await _unitOfWork.Commit();
    }
}
