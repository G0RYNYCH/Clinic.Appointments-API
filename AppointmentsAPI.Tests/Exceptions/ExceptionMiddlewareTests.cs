using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Appointments_API.Tests.Extensions;
using AppointmentsAPI.Exceptions;
using AppointmentsAPI.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Appointments_API.Tests.Exceptions;

public class ExceptionMiddlewareTests
{
    private readonly WebApplication _app;
    private readonly Mock<ILogger<ExceptionMiddleware>> _logger;
    
    public ExceptionMiddlewareTests()
    {
        _logger = new Mock<ILogger<ExceptionMiddleware>>();

        var builder = WebApplication.CreateBuilder();
        builder.Services.AddLogging();
        
        _app = builder.Build();
        
        _app.UseCustomExceptionMiddleware();

        _app.MapGet("/", () =>
        {
            throw new Exception("Test exception");
        });
        
        _= _app.RunAsync("http://localhost:8080/");
    }
    
    [Fact]
    public async Task ExceptionMiddlewareTest_Returns()
    {
        //Arrange
        var httpClient = new HttpClient();
        
        //Act
        var httpResult = await httpClient.GetAsync("http://localhost:8080/");
        var response = JsonSerializer.Deserialize<Response>(await httpResult.Content.ReadAsStringAsync());
        
        //Assert
        response.Should().NotBeNull();
        response!.message.Should().Be("Some error occured");
        //TODO: _logger.VerifyLogging("Test exception", LogLevel.Information, Times.Once());
        
        await _app.StopAsync();
    }
    
    class Response
    {
        public int statusCode { get; set; }
        public string message { get; set; }
    }
}