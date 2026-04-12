using Microsoft.AspNetCore.Mvc;
using ResidentialExpenses.API.Attributes;
using ResidentialExpenses.Application.UseCases.Transactions.GetAll;
using ResidentialExpenses.Application.UseCases.Transactions.GetTotalsByPerson;
using ResidentialExpenses.Application.UseCases.Transactions.Register;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.API.Controllers;

[AuthenticatedUser]
public class TransactionController : ResidentialExpensesBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseApiJson<ResponseRegisteredTransactionJson>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseApiJson<object>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ResponseApiJson<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterTransactionUseCase useCase,
        [FromBody] RequestRegisterTransactionJson request)
    {
        var result = await useCase.Execute(request);
        return SuccessCreated(result);
    }

    [HttpGet("person/{personId}")]
    [ProducesResponseType(typeof(ResponseApiJson<List<ResponseShortTransactionJson>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseApiJson<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllByPerson(
        [FromServices] IGetAllTransactionsUseCase useCase,
        [FromRoute] long personId)
    {
        var result = await useCase.Execute(personId);
        return SuccessOk(result);
    }

    [HttpGet("totals")]
    [ProducesResponseType(typeof(ResponseApiJson<ResponseTotalsSummaryJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTotalsByPerson(
        [FromServices] IGetTotalsByPersonUseCase useCase)
    {
        var result = await useCase.Execute();
        return SuccessOk(result);
    }
}
