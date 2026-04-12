using AutoMapper;
using ResidentialExpenses.Communication.Enums;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Communication.Responses;
using ResidentialExpenses.Domain.Enums;
using ResidentialExpenses.Domain.Repositories;
using ResidentialExpenses.Domain.Repositories.Category;
using ResidentialExpenses.Domain.Repositories.Person;
using ResidentialExpenses.Domain.Repositories.Transaction;
using ResidentialExpenses.Domain.Services.LoggedUser;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace ResidentialExpenses.Application.UseCases.Transactions.Register;

public class RegisterTransactionUseCase : IRegisterTransactionUseCase
{
    private readonly ITransactionWriteOnlyRepository _writeOnlyRepository;
    private readonly IPersonReadOnlyRepository _personReadOnlyRepository;
    private readonly ICategoryReadOnlyRepository _categoryReadOnlyRepository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterTransactionUseCase(
        ITransactionWriteOnlyRepository writeOnlyRepository,
        IPersonReadOnlyRepository personReadOnlyRepository,
        ICategoryReadOnlyRepository categoryReadOnlyRepository,
        ILoggedUser loggedUser,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _personReadOnlyRepository = personReadOnlyRepository;
        _categoryReadOnlyRepository = categoryReadOnlyRepository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseRegisteredTransactionJson> Execute(RequestRegisterTransactionJson request)
    {
        var loggedUser = await _loggedUser.Get();

        await Validate(request, loggedUser.Id);

        var transaction = _mapper.Map<Domain.Entities.Transaction>(request);

        await _writeOnlyRepository.Add(transaction);

        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisteredTransactionJson>(transaction);
    }

    private async Task Validate(RequestRegisterTransactionJson request, long userId)
    {
        var validator = new RegisterTransactionValidator();
        var result = validator.Validate(request);

        var person = await _personReadOnlyRepository.GetByIdAndUserId(request.PersonId, userId);
        if (person is null)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceErrorMessages.PERSON_NOT_FOUND));
        }
        else
        {
            if (person.Age < 18 && request.Type != TransactionTypeJson.Expense)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceErrorMessages.TRANSACTION_MINOR_ONLY_EXPENSE));
        }

        var category = await _categoryReadOnlyRepository.GetById(request.CategoryId);
        if (category is null)
        {
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceErrorMessages.TRANSACTION_CATEGORY_NOT_FOUND));
        }
        else
        {
            var transactionType = (TransactionType)request.Type;
            var isCompatible = category.Purpose == CategoryPurpose.Both
                || (transactionType == TransactionType.Expense && category.Purpose == CategoryPurpose.Expense)
                || (transactionType == TransactionType.Income && category.Purpose == CategoryPurpose.Income);

            if (!isCompatible)
                result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceErrorMessages.TRANSACTION_CATEGORY_PURPOSE_MISMATCH));
        }

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
