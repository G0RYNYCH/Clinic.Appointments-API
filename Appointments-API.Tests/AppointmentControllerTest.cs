using Appointments_API.Controllers;
using Appointments_API.Interfaces;
using Appointments_API.Models;
using Appointments_API.Models.Dto;
using Appointments_API.Tests.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Appointments_API.Tests;

public class AppointmentControllerTest
{
    private AppointmentController _appointmentController;
    private readonly Mock<IAppointmentService> _appointmentService;
    private readonly Mock<ILogger<AppointmentController>> _logger;
    private readonly CancellationToken _cancelationToken;

    public AppointmentControllerTest()
    {
        _appointmentService = new Mock<IAppointmentService>();
        _logger = new Mock<ILogger<AppointmentController>>();
        _cancelationToken = new CancellationToken();

        var mockHttpContext = new DefaultHttpContext();
        mockHttpContext.RequestAborted = _cancelationToken;

        _appointmentController = new AppointmentController(_appointmentService.Object, _logger.Object)// TODO: new instanse? 
        {
            ControllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext
            }
        };
    }

    #region Ctor

    [Fact]
    public void AppointmentControllerCtor_WithNullAppointmentService_ThrowsNullArgumentExcception()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => new AppointmentController(null, _logger.Object));
    }

    [Fact]
    public void AppointmentControllerCtor_WithNullLogger_ThrowsNullArgumentExcception()
    {
        // Arrange
        // Act
        // Assert
        Assert.Throws<ArgumentNullException>(() => new AppointmentController(_appointmentService.Object, null));
    }

    [Fact]
    public void AppointmentControllerCtor_WithNotNullArgs_CreatesInstance()
    {
        // Arrange
        // Act
        var result = new AppointmentController(_appointmentService.Object, _logger.Object);

        // Assert
        result.Should().NotBeNull();
    }

    #endregion

    #region GetAppointments

    [Fact]
    public async Task GetAppointments_ValidModel_ReturnsResult()
    {
        // Arrange
        var searchDto = new SearchDto()
        {
            PageSize = 10,
            PageNumber = 1,
        };

        var testAppointment = new Appointment()
        {
            PatientId = Guid.NewGuid()
        };

        var searchResult = new List<Appointment>()
        {
            testAppointment
        };

        _appointmentService.Setup(x => x.SearchAsync(searchDto, _cancelationToken))
            .ReturnsAsync(searchResult);

        // Act
        var result = (await _appointmentController.GetAppointments(searchDto)) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        _appointmentService.Verify(x => x.SearchAsync(It.IsAny<SearchDto>(), It.IsAny<CancellationToken>()), Times.Once());
        _logger.VerifyLogging("GetAppointments method is called", LogLevel.Information, Times.Once());
        _logger.VerifyLogging("GetAppointments method succeeded", LogLevel.Information, Times.Once());

        var appointments = result!.Value as IEnumerable<Appointment>;
        appointments.Should().NotBeNullOrEmpty();
        appointments.Should().HaveCount(1);

        var appointment = appointments!.First();
        appointment.PatientId.Should().Be(testAppointment.PatientId);
    }

    #endregion

    #region GetById

    [Fact]
    public async Task GetById_ExistingId_ReturnsAppointment()
    {
        // Arrange
        var appointmnentId = Guid.NewGuid();
        var searchResult = new Appointment()
        {
            Id = appointmnentId,
        };

        _appointmentService.Setup(x => x.GetByIdAsync(appointmnentId, _cancelationToken))
            .ReturnsAsync(searchResult);

        // Act
        var result = (await _appointmentController.GetById(appointmnentId)) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        _appointmentService.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());
        _logger.VerifyLogging("GetById method is called", LogLevel.Information, Times.Once());
        _logger.VerifyLogging("GetById method succeeded", LogLevel.Information, Times.Once());
        _logger.VerifyLogging($"Appointment with Id = {appointmnentId} is null", LogLevel.Information, Times.Never());

        var appointment = result!.Value as Appointment;
        appointment.Should().NotBeNull();
        appointment!.Id.Should().Be(searchResult.Id);
    }

    [Fact]
    public async Task GetById_IsNotExistingId_ReturnsBadRequest()
    {
        // Arrange
        var appointmnentId = Guid.NewGuid();

        // Act
        var result = await _appointmentController.GetById(appointmnentId);

        // Assert
        result.Should().NotBeNull();
        _appointmentService.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());
        _logger.VerifyLogging("GetById method is called", LogLevel.Information, Times.Once());
        _logger.VerifyLogging($"Appointment with Id = {appointmnentId} is null", LogLevel.Information, Times.Once());
        _logger.VerifyLogging("GetById method succeeded", LogLevel.Information, Times.Never());
    }

    #endregion
}
