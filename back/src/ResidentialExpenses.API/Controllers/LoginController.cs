using Microsoft.AspNetCore.Mvc;
using ResidentialExpenses.Application.UseCases.Users.Login;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.API.Controllers;

public class LoginController : ResidentialExpensesBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseApiJson<ResponseRegisteredUserJson>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Login(
        [FromServices] IDoLoginUseCase useCase,
        [FromBody] RequestLoginJson request)
    {
        var result = await useCase.Execute(request);
        return SuccessOk(result);
    }
}
