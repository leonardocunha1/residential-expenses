using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ResidentialExpenses.Communication.Responses;
using ResidentialExpenses.Exceptions;
using ResidentialExpenses.Exceptions.ExceptionsBase;

namespace ResidentialExpenses.API.Filters;

public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if (context.Exception is ResidentialExpensesException)
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnknowError(context);
        }

    }

    private void HandleProjectException(ExceptionContext context)
    {
        var residentialExpensesException = (ResidentialExpensesException)context.Exception;
        context.HttpContext.Response.StatusCode = residentialExpensesException.StatusCode;
        context.Result = new ObjectResult(CreateErrorEnvelope(residentialExpensesException.GetErrors()));
        context.ExceptionHandled = true;
    }

    private void ThrowUnknowError(ExceptionContext context)
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(CreateErrorEnvelope([ResourceErrorMessages.UNKNOW_ERROR]));
        context.ExceptionHandled = true;
    }

    private static ResponseApiJson<object?> CreateErrorEnvelope(List<string> errors)
    {
        return new ResponseApiJson<object?>
        {
            Success = false,
            Data = null,
            Errors = errors,
            Metadata = new ResponseMetadataJson()
        };
    }
}
