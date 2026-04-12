using Microsoft.AspNetCore.Mvc;
using ResidentialExpenses.API.Attributes;
using ResidentialExpenses.Application.UseCases.Categories.GetAll;
using ResidentialExpenses.Application.UseCases.Categories.Register;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.API.Controllers;

[AuthenticatedUser]
public class CategoryController : ResidentialExpensesBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseApiJson<ResponseRegisteredCategoryJson>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseApiJson<object>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterCategoryUseCase useCase,
        [FromBody] RequestRegisterCategoryJson request)
    {
        var result = await useCase.Execute(request);
        return SuccessCreated(result);
    }

    [HttpGet]
    [ProducesResponseType(typeof(ResponseApiJson<List<ResponseShortCategoryJson>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(
        [FromServices] IGetAllCategoriesUseCase useCase)
    {
        var result = await useCase.Execute();
        return SuccessOk(result);
    }
}
