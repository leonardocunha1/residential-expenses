using AutoMapper;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Communication.Responses;
using ResidentialExpenses.Domain.Repositories;
using ResidentialExpenses.Domain.Repositories.Category;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace ResidentialExpenses.Application.UseCases.Categories.Register;

public class RegisterCategoryUseCase : IRegisterCategoryUseCase
{
    private readonly ICategoryWriteOnlyRepository _writeOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RegisterCategoryUseCase(
        ICategoryWriteOnlyRepository writeOnlyRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _writeOnlyRepository = writeOnlyRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseRegisteredCategoryJson> Execute(RequestRegisterCategoryJson request)
    {
        Validate(request);

        var category = _mapper.Map<Domain.Entities.Category>(request);

        await _writeOnlyRepository.Add(category);

        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisteredCategoryJson>(category);
    }

    private static void Validate(RequestRegisterCategoryJson request)
    {
        var validator = new RegisterCategoryValidator();

        var result = validator.Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
