using AppointmentsAPI.Models;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using AppointmentsAPI.Exceptions;

namespace AppointmentsAPI.Extensions;

public static class ExceptionMiddlewareExtension
{
    //public static void ConfigureExceptionHandler(this IApplicationBuilder app, ILogger logger)//TODO: Serilog.Core.Logger/ILogger
    //{
    //    app.UseExceptionHandler(appError =>
    //    {
    //        appError.Run(async context =>
    //        {
    //            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    //            context.Response.ContentType = "application/json";

    //            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
    //            if (contextFeature is not null)
    //            {
    //                logger.LogError(contextFeature.Error.Message, contextFeature.Error);

    //                await context.Response.WriteAsync(new ErrorDetails
    //                {
    //                    StatusCode = context.Response.StatusCode,
    //                    Message = "Server Error"
    //                }.ToString());
    //            }
    //        });
    //    });
    //}

    public static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}
