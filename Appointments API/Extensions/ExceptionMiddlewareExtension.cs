using Appointments_API.Exceptions;
using Appointments_API.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace Appointments_API.Extensions;

public static class ExceptionMiddlewareExtension
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, Serilog.Core.Logger logger)//TODO: Serilog.Core.Logger/ILogger
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature is not null)
                {
                    logger.Error(contextFeature.Error.Message, contextFeature.Error);

                    await context.Response.WriteAsync(new ErrorDetails
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = "Server Error"
                    }.ToString());
                }
            });
        });
    }

    public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
