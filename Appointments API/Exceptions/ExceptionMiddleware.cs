using System.Net;

namespace Appointments_API.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception.Message, exception);
            await HandleExceptionAsync(httpContext);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext)
    {
        await httpContext.Response.WriteAsJsonAsync(new ErrorDetails()
        {
            StatusCode = httpContext.Response.StatusCode,
            Message = "Some error occured"
        });
    }
}
