using Appointments_API.Controllers;
using Appointments_API.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace Appointments_API.Tests;

public class AppointmentControllerTest : IClassFixture<AppointmentController>
{
    private readonly AppointmentController _appointmentController;
    private readonly Mock<IAppointmentService> _appointmentService;
    private readonly Mock<ILogger<AppointmentController>> _logger;

    public AppointmentControllerTest(AppointmentController appointmentController, 
        Mock<IAppointmentService> appointmentService, Mock<ILogger> logger)
    {
        _appointmentController = new AppointmentController(_appointmentService.Object, _logger.Object);
        _appointmentService = new Mock<IAppointmentService>();
        _logger = new Mock<ILogger<AppointmentController>>();
    }

    [Fact]
    public void AppointmentControllerCtor_WithNullAppointmentService_ThrowsNullArgumentExcception()
    {
        Assert.Throws<ArgumentNullException>(() => new AppointmentController(null, _logger.Object));
    }

    [Fact]
    public void AppointmentControllerCtor_WithNullLogger_ThrowsNullArgumentExcception()
    {
        Assert.Throws<ArgumentNullException>(() => new AppointmentController(_appointmentService.Object, null));
    }

    [Fact]
    public void AppointmentControllerCtor_WithNotNullArgs_CreatesInstance()
    {
        Assert.Throws<ArgumentNullException>(() => new AppointmentController(_appointmentService.Object, _logger.Object));
    }
}
