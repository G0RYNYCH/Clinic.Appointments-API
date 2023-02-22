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

namespace Appointments_API.Tests.Controllers;

public class AppointmentControllerTests
{
    private readonly AppointmentController _appointmentController;
    private readonly Mock<IAppointmentService> _appointmentService;
    private readonly Mock<ILogger<AppointmentController>> _logger;
    private readonly CancellationToken _cancelationToken;

    public AppointmentControllerTests()
    {
        _appointmentService = new Mock<IAppointmentService>();
        _logger = new Mock<ILogger<AppointmentController>>();
        _cancelationToken = new CancellationToken();

        var mockHttpContext = new DefaultHttpContext();
        mockHttpContext.RequestAborted = _cancelationToken;

        _appointmentController = new AppointmentController(_appointmentService.Object, _logger.Object)
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
        var appointmentId = Guid.NewGuid();
        var searchResult = new Appointment()
        {
            Id = appointmentId,
        };

        _appointmentService.Setup(x => x.GetByIdAsync(appointmentId, _cancelationToken))
            .ReturnsAsync(searchResult);

        // Act
        var result = (await _appointmentController.GetById(appointmentId)) as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        _appointmentService.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());
        _logger.VerifyLogging("GetById method is called", LogLevel.Information, Times.Once());
        _logger.VerifyLogging("GetById method succeeded", LogLevel.Information, Times.Once());
        _logger.VerifyLogging($"Appointment with Id = {appointmentId} is null", LogLevel.Information, Times.Never());

        var appointment = result!.Value as Appointment;
        appointment.Should().NotBeNull();
        appointment!.Id.Should().Be(searchResult.Id);
    }

    [Fact]
    public async Task GetById_IsNotExistingId_ReturnsBadRequest()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();

        // Act
        var result = await _appointmentController.GetById(appointmentId);

        // Assert
        result.Should().NotBeNull();
        _appointmentService.Verify(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once());
        _logger.VerifyLogging("GetById method is called", LogLevel.Information, Times.Once());
        _logger.VerifyLogging($"Appointment with Id = {appointmentId} is null", LogLevel.Information, Times.Once());
        _logger.VerifyLogging("GetById method succeeded", LogLevel.Information, Times.Never());
    }

    #endregion

    #region Create

    [Fact]
    public async Task Create_AppointmentDto_Success()
    {
        // Arrange
        var appointmentDto = new AppointmentDto();

        var createResult = new Appointment();

        // Act
        var result = await _appointmentController.Create(appointmentDto);

        // Assert
        result.Should().NotBeNull();
        _appointmentService.Verify(x => x.CreateAsync(appointmentDto, _cancelationToken), Times.Once());
        _logger.VerifyLogging("Create method is called", LogLevel.Information, Times.Once());
        _logger.VerifyLogging("Create method succeeded", LogLevel.Information, Times.Once());
    }

    #endregion

    #region Update

    [Fact]
    public async Task Update_ExistingId_Succeess()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var updateAppointmentDto = new UpdateAppointmentDto();
        var getByIdResult = new Appointment()
        {
            Id = appointmentId,
        };

        _appointmentService.Setup(x => x.GetByIdAsync(appointmentId, _cancelationToken))
            .ReturnsAsync(getByIdResult);

        // Act
        var result = await _appointmentController.Update(appointmentId, updateAppointmentDto);

        // Assert
        result.Should().NotBeNull();
        _appointmentService.Verify(x => x.UpdateAsync(appointmentId, updateAppointmentDto, _cancelationToken), Times.Once());
        _logger.VerifyLogging("Update method is called", LogLevel.Information, Times.Once());
        _logger.VerifyLogging("Update method succeeded", LogLevel.Information, Times.Once());
        _logger.VerifyLogging($"Appointment with Id = {appointmentId} is null", LogLevel.Information, Times.Never());
    }

    [Fact]
    public async Task Update_IsNotExistingId_ReturnsBadRequest()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var updateAppointmentDto = new UpdateAppointmentDto();

        // Act
        var result = await _appointmentController.Update(appointmentId, updateAppointmentDto);

        // Assert
        result.Should().NotBeNull();
        _appointmentService.Verify(x => x.UpdateAsync(appointmentId, updateAppointmentDto, _cancelationToken), Times.Never());
        _logger.VerifyLogging("Update method is called", LogLevel.Information, Times.Once());
        _logger.VerifyLogging($"Appointment with Id = {appointmentId} is null", LogLevel.Information, Times.Once());
        _logger.VerifyLogging("Update method succeeded", LogLevel.Information, Times.Never());
    }

    #endregion

    #region Delete

    [Fact]
    public async Task Delete_ExistingId_Succeess()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var getByIdResult = new Appointment()
        {
            Id = appointmentId,
        };

        _appointmentService.Setup(x => x.GetByIdAsync(appointmentId, _cancelationToken))
            .ReturnsAsync(getByIdResult);

        // Act
        var result = await _appointmentController.Delete(appointmentId);

        // Assert
        result.Should().NotBeNull();
        _appointmentService.Verify(x => x.DeleteAsync(appointmentId, _cancelationToken), Times.Once());
        _logger.VerifyLogging("Delete method is called", LogLevel.Information, Times.Once());
        _logger.VerifyLogging("Delete method succeeded", LogLevel.Information, Times.Once());
        _logger.VerifyLogging($"Appointment with Id = {appointmentId} is null", LogLevel.Information, Times.Never());
    }

    [Fact]
    public async Task Delete_IsNotExistingId_ReturnsBadRequest()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();

        // Act
        var result = await _appointmentController.Delete(appointmentId);

        // Assert
        result.Should().NotBeNull();
        _appointmentService.Verify(x => x.DeleteAsync(appointmentId, _cancelationToken), Times.Never());
        _logger.VerifyLogging("Delete method is called", LogLevel.Information, Times.Once());
        _logger.VerifyLogging($"Appointment with Id = {appointmentId} is null", LogLevel.Information, Times.Once());
        _logger.VerifyLogging("Delete method succeeded", LogLevel.Information, Times.Never());
    }

    #endregion
}
