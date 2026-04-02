using AutoMapper;
using ResidentialExpenses.Communication.Responses;
using ResidentialExpenses.Domain.Repositories.Person;
using ResidentialExpenses.Domain.Services.LoggedUser;

namespace ResidentialExpenses.Application.UseCases.People.GetAll;

public class GetAllPeopleUseCase : IGetAllPeopleUseCase
{
    private readonly IPersonReadOnlyRepository _readOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetAllPeopleUseCase(
        IPersonReadOnlyRepository readOnlyRepository,
        ILoggedUser loggedUser,
        IMapper mapper)
    {
        _readOnlyRepository = readOnlyRepository;
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<List<ResponseShortPersonJson>> Execute()
    {
        var loggedUser = await _loggedUser.Get();

        var people = await _readOnlyRepository.GetAllByUserId(loggedUser.Id);

        return _mapper.Map<List<ResponseShortPersonJson>>(people);
    }
}
