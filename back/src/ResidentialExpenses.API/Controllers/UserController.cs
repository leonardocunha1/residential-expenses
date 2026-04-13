using Microsoft.AspNetCore.Mvc;
using ResidentialExpenses.API.Attributes;
using ResidentialExpenses.Application.UseCases.Users.Delete;
using ResidentialExpenses.Application.UseCases.Users.Profile;
using ResidentialExpenses.Application.UseCases.Users.Register;
using ResidentialExpenses.Application.UseCases.Users.Update;
using ResidentialExpenses.Communication.Requests;
using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.API.Controllers;

public class UserController : ResidentialExpensesBaseController
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        var result = await useCase.Execute(request);
        return Created(string.Empty, result);
    }

    [HttpGet]
    [AuthenticatedUser]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetProfile(
        [FromServices] IGetUserProfileUseCase useCase)
    {
        var result = await useCase.Execute();
        return Ok(result);
    }

    [HttpPut]
    [AuthenticatedUser]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Update(
        [FromServices] IUpdateUserUseCase useCase,
        [FromBody] RequestUpdateUserJson request)
    {
        var result = await useCase.Execute(request);
        return Ok(result);
    }

    [HttpDelete]
    [AuthenticatedUser]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Delete(
        [FromServices] IDeleteUserUseCase useCase)
    {
        await useCase.Execute();
        return NoContent();
    }
}
