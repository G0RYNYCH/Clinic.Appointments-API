using System.Net;

namespace Appointments_API.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly Serilog.Core.Logger _logger;//TODO: Serilog.Core.Logger type

    public ExceptionMiddleware(RequestDelegate next, Serilog.Core.Logger logger)
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
            _logger.Error(exception.Message, exception);
            await HandleExceptionAsync(httpContext, exception);
        }
    }

    private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        await httpContext.Response.WriteAsync(new ErrorDetails()
        {
            StatusCode = httpContext.Response.StatusCode,
            Message = "Internal Server Error"
        }.ToString());
    }
}
