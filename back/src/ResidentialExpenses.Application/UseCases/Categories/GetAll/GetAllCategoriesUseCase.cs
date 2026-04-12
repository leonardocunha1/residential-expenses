using AutoMapper;
using ResidentialExpenses.Communication.Responses;
using ResidentialExpenses.Domain.Repositories.Category;

namespace ResidentialExpenses.Application.UseCases.Categories.GetAll;

public class GetAllCategoriesUseCase : IGetAllCategoriesUseCase
{
    private readonly ICategoryReadOnlyRepository _readOnlyRepository;
    private readonly IMapper _mapper;

    public GetAllCategoriesUseCase(
        ICategoryReadOnlyRepository readOnlyRepository,
        IMapper mapper)
    {
        _readOnlyRepository = readOnlyRepository;
        _mapper = mapper;
    }

    public async Task<List<ResponseShortCategoryJson>> Execute()
    {
        var categories = await _readOnlyRepository.GetAll();

        return _mapper.Map<List<ResponseShortCategoryJson>>(categories);
    }
}
