using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace ResidentialExpenses.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ResidentialExpensesException residentialExpensesException)
        {
            HandleProjectException(context, residentialExpensesException);
        }
        else
        {
            ThrowUnknowError(context);
        }
    }

    private static void HandleProjectException(ExceptionContext context, ResidentialExpensesException exception)
    {
        context.HttpContext.Response.StatusCode = exception.StatusCode;
        context.Result = new ObjectResult(CreateProblem(exception.StatusCode, exception.GetErrors()));
        context.ExceptionHandled = true;
    }

    private static void ThrowUnknowError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(CreateProblem(StatusCodes.Status500InternalServerError, [ResourceErrorMessages.UNKNOW_ERROR]));
        context.ExceptionHandled = true;
    }

    private static ProblemDetails CreateProblem(int statusCode, List<string> errors)
    {
        var problem = new ProblemDetails
        {
            Status = statusCode,
            Title = ReasonPhrases.Get(statusCode),
        };
        problem.Extensions["errors"] = errors;
        return problem;
    }
}

internal static class ReasonPhrases
{
    public static string Get(int statusCode) => statusCode switch
    {
        StatusCodes.Status400BadRequest => "Bad Request",
        StatusCodes.Status401Unauthorized => "Unauthorized",
        StatusCodes.Status403Forbidden => "Forbidden",
        StatusCodes.Status404NotFound => "Not Found",
        StatusCodes.Status409Conflict => "Conflict",
        StatusCodes.Status500InternalServerError => "Internal Server Error",
        _ => "Error",
    };
}
