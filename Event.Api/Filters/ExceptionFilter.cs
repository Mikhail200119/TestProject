using Event.Api.Models.Response;
using Event.Bll.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Event.Api.Filters;

public class ExceptionFilter :  IExceptionFilter
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ExceptionFilter(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;
        var statusCode = GetStatusCode(exception);
        var response = GetExceptionResponse();
        
        if (response is DevExceptionResponse devExceptionResponse)
        {
            devExceptionResponse.Message = exception.Message;
            devExceptionResponse.Type = exception.GetType().ToString();
            devExceptionResponse.StackTrace = exception.StackTrace;
            devExceptionResponse.InnerException = exception.InnerException;
        }
        else
        {
            var isUnhandledException = statusCode == StatusCodes.Status500InternalServerError;
            var message = isUnhandledException ? "Something went wrong on the server..." : exception.Message;
            response.Message = message;
        }

        response.StatusCode = statusCode;

        context.Result = new ObjectResult(response)
        {
            StatusCode = statusCode
        };
    }

    private static int GetStatusCode(Exception exception) =>
        exception switch
        {
            NotFoundException => StatusCodes.Status404NotFound,
            PermissionsException => StatusCodes.Status401Unauthorized,
            _ => StatusCodes.Status500InternalServerError
        };

    private ExceptionResponse GetExceptionResponse()
    {
        return _webHostEnvironment.IsDevelopment() ? new DevExceptionResponse() : new ExceptionResponse();
    }
}