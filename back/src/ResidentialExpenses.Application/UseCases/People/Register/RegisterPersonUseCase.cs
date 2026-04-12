using AutoMapper;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Communication.Responses;
using ResidentialExpenses.Domain.Repositories;
using ResidentialExpenses.Domain.Repositories.Person;
using ResidentialExpenses.Domain.Repositories.User;
using ResidentialExpenses.Domain.Services.LoggedUser;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace ResidentialExpenses.Application.UseCases.People.Register;

public class RegisterPersonUseCase : IRegisterPersonUseCase
{
    private readonly IPersonWriteOnlyRepository _writeOnlyRepository;
    private readonly IUserUpdateOnlyRepository _userUpdateOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterPersonUseCase(
        IPersonWriteOnlyRepository writeOnlyRepository,
        IUserUpdateOnlyRepository userUpdateOnlyRepository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _userUpdateOnlyRepository = userUpdateOnlyRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseRegisteredPersonJson> Execute(RequestRegisterPersonJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();
        var trackedUser = await _userUpdateOnlyRepository.GetById(loggedUser.Id);

        var person = _mapper.Map<Domain.Entities.Person>(request);
        person.Users.Add(trackedUser);

        await _writeOnlyRepository.Add(person);

        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisteredPersonJson>(person);
    }

    private static void Validate(RequestRegisterPersonJson request)
    {
        var validator = new RegisterPersonValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
