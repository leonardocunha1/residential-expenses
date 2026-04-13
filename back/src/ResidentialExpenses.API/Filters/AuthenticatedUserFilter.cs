using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using ResidentialExpenses.Domain.Repositories.User;
using ResidentialExpenses.Domain.Security.Tokens;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace ResidentialExpenses.API.Filters;

public class AuthenticatedUserFilter : IAsyncAuthorizationFilter
{
    private readonly IAccessTokenValidator _accessTokenValidator;
    private readonly IUserReadOnlyRepository _repository;

    public AuthenticatedUserFilter(IAccessTokenValidator accessTokenValidator, IUserReadOnlyRepository repository)
    {
        _accessTokenValidator = accessTokenValidator;
        _repository = repository;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        try
        {
            var token = TokenOnRequest(context);

            var userId = _accessTokenValidator.ValidateAndGetUserIdentifier(token);

            var exists = await _repository.ExistActiveUserById(userId);
            if (!exists)
            {
                throw new ResidentialExpensesUnauthorizedException(ResourceErrorMessages.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);
            }
        }
        catch (SecurityTokenExpiredException)
        {
            context.Result = new UnauthorizedObjectResult(CreateProblem(ResourceErrorMessages.TOKEN_EXPIRED, tokenIsExpired: true));
        }
        catch (ResidentialExpensesException ex)
        {
            context.Result = new UnauthorizedObjectResult(CreateProblem(ex.Message));
        }
        catch
        {
            context.Result = new UnauthorizedObjectResult(CreateProblem(ResourceErrorMessages.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE));
        }
    }

    private static ProblemDetails CreateProblem(string error, bool tokenIsExpired = false)
    {
        var problem = new ProblemDetails
        {
            Status = StatusCodes.Status401Unauthorized,
            Title = "Unauthorized",
        };
        problem.Extensions["errors"] = new List<string> { error };
        if (tokenIsExpired)
        {
            problem.Extensions["tokenIsExpired"] = true;
        }
        return problem;
    }

    private static string TokenOnRequest(AuthorizationFilterContext context)
    {
        var authentication = context.HttpContext.Request.Headers.Authorization.ToString();
        if (string.IsNullOrWhiteSpace(authentication) || !authentication.StartsWith("Bearer "))
        {
            throw new ResidentialExpensesUnauthorizedException(ResourceErrorMessages.NO_TOKEN);
        }

        return authentication["Bearer ".Length..].Trim();
    }
}
