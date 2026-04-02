using AutoMapper;
using ResidentialExpenses.Communication.Responses;
using ResidentialExpenses.Domain.Repositories.Person;
using ResidentialExpenses.Domain.Repositories.Transaction;
using ResidentialExpenses.Domain.Services.LoggedUser;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace ResidentialExpenses.Application.UseCases.Transactions.GetAll;

public class GetAllTransactionsUseCase : IGetAllTransactionsUseCase
{
    private readonly ITransactionReadOnlyRepository _readOnlyRepository;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetAllTransactionsUseCase(
        ITransactionReadOnlyRepository readOnlyRepository,
        IPersonReadOnlyRepository personReadOnlyRepository,
        ILoggedUser loggedUser,
        IMapper mapper)
    {
        _readOnlyRepository = readOnlyRepository;
        _personReadOnlyRepository = personReadOnlyRepository;
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<List<ResponseShortTransactionJson>> Execute(long personId)
    {
        var loggedUser = await _loggedUser.Get();

        var person = await _personReadOnlyRepository.GetByIdAndUserId(personId, loggedUser.Id);

        if (person is null)
            throw new NotFoundException(ResourceErrorMessages.PERSON_NOT_FOUND);

        var transactions = await _readOnlyRepository.GetAllByPersonId(personId);

        return _mapper.Map<List<ResponseShortTransactionJson>>(transactions);
    }
}
