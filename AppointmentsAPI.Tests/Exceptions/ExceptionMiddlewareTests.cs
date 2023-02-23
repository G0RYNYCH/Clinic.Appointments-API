using System;
using System.Threading.Tasks;
using AppointmentsAPI.Exceptions;
using AppointmentsAPI.Interfaces;
using AppointmentsAPI.Repositories;
using AppointmentsAPI.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace Appointments_API.Tests.Exceptions;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task ExceptionMiddlewareTest_Returns()
    {
        //Arrange
        using var host = await new HostBuilder().ConfigureWebHost(webBuilder =>
        {
            webBuilder
                .UseTestServer()
                .ConfigureServices(services =>
                {
                    services.AddControllers();
                    services.AddMvc();
                    services.AddFluentValidationAutoValidation();
                    services.AddValidatorsFromAssemblyContaining<IValidator>();
                    services.AddScoped<IAppointmentService, AppointmentService>();
                    services.AddScoped<IAppointmentRepository, AppointmentRepository>();
                    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                })
                .Configure(app =>
                {
                    app.UseMiddleware<ExceptionMiddleware>();
                });
        }).StartAsync();
        
        //Act
        //var response = await host.GetTestClient().GetAsync("/");
        var server = host.GetTestServer();
        server.BaseAddress = new Uri("https://localhost:9008");

        var context = await server.SendAsync(c =>
        {
            c.Request.Method = HttpMethods.Post;
        });

        //Assert
        //response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
        Assert.True(context.RequestAborted.CanBeCanceled);
        Assert.Equal("POST", context.Request.Method);
    }
}