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
        var errorResponse = new ResponseErrorJson(residentialExpensesException.GetErrors());
        context.HttpContext.Response.StatusCode = residentialExpensesException.StatusCode;
        context.Result = new ObjectResult(errorResponse);
        context.ExceptionHandled = true;
    }

    private void ThrowUnknowError(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson(ResourceErrorMessages.UNKNOW_ERROR);
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Result = new ObjectResult(errorResponse);
        context.ExceptionHandled = true;
    }
}
