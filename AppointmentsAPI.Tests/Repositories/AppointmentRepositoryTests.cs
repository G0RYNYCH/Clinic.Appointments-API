using AppointmentsAPI.Interfaces;
using AppointmentsAPI.Models;
using AppointmentsAPI.Models.Dto;
using AppointmentsAPI.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Appointments_API.Tests.Repositories;

public class AppointmentRepositoryTests
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly Mock<AppointmentDbContext> _context;
    private readonly CancellationToken _cancellationToken;

    public AppointmentRepositoryTests()
    {
        var dbContextOptions = new DbContextOptions<AppointmentDbContext>();
        _context = new Mock<AppointmentDbContext>(dbContextOptions);
        _cancellationToken = new CancellationToken();
        _appointmentRepository = new AppointmentRepository(_context.Object);
    }

    #region Ctor

    [Fact]
    public void AppointmentRepositoryCtor_NullParameter_ThrowsArgumentNullException()
    {
        //Arrange
        //Act
        //Assert
        Assert.Throws<ArgumentNullException>(() => new AppointmentRepository(null));
    }

    [Fact]
    public void AppointmentRepositoryCtor_NotNullParameter_CreatesInstance()
    {
        //Arrange
        //Act
        var result = new AppointmentRepository(_context.Object);

        //Assert
        result.Should().NotBeNull();
    }

    #endregion

    #region SearchAsync

    [Fact]
    public async Task SearchAsync_ValidParams_ReturnsAppointments()
    {
        // Arrange
        var appointments = new List<Appointment>()
        {
            new Appointment(),
        };
        var searchDto = new SearchDto()
        {
            PageSize = 1,
            PageNumber = 1,
        };

        _context.Setup(x => x.Set<Appointment>())
            .ReturnsDbSet(appointments);

        //Act
        var result = await _appointmentRepository.SearchAsync(searchDto, _cancellationToken);

        //Assert
        _context.Verify(x => x.Set<Appointment>(), Times.Once());
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(1);
    }

    #endregion

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_ExistingId_ReturnsAppointment()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var appointment = new Appointment()
        {
            Id = appointmentId,
        };
        var appointments = new List<Appointment>()
        {
            appointment,
        };

        _context.Setup(x => x.Set<Appointment>())
            .ReturnsDbSet(appointments);

        //Act
        var result = await _appointmentRepository.GetByIdAsync(appointmentId, _cancellationToken);

        //Assert
        _context.Verify(x => x.Set<Appointment>(), Times.Once());
        result.Should().NotBeNull();
        result.Id.Should().Be(appointment.Id);
    }

    [Fact]
    public async Task GetByIdAsync_NotExistingId_ReturnsNull()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var appointment = new Appointment();
        var appointments = new List<Appointment>()
        {
            appointment,
        };

        _context.Setup(x => x.Set<Appointment>())
            .ReturnsDbSet(appointments);

        //Act
        var result = await _appointmentRepository.GetByIdAsync(appointmentId, _cancellationToken);

        //Assert
        _context.Verify(x => x.Set<Appointment>(), Times.Once());
        result.Should().BeNull();
    }

    #endregion

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_ValidParams_Succeeded()
    {
        // Arrange
        var appointment = new Appointment();

        var dbSetMock = new Mock<DbSet<Appointment>>();

        _context.Setup(x => x.Set<Appointment>())
            .Returns(dbSetMock.Object);

        //Act
        await _appointmentRepository.CreateAsync(appointment, _cancellationToken);

        //Assert
        _context.Verify(x => x.Set<Appointment>(), Times.Once());
        dbSetMock.Verify(x => x.AddAsync(appointment, _cancellationToken), Times.Once());
        _context.Verify(x => x.SaveChangesAsync(_cancellationToken), Times.Once());
    }

    #endregion

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_ValidParams_Succeeded()
    {
        // Arrange
        var appointment = new Appointment();

        //_context.Setup(x => x.Update())
        //    .Returns();

        //Act
        await _appointmentRepository.UpdateAsync(appointment, _cancellationToken);

        //Assert
        _context.Verify(x => x.Update(appointment), Times.Once());
        _context.Verify(x => x.SaveChangesAsync(_cancellationToken), Times.Once());
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_ValidParams_Succeeded()
    {
        // Arrange
        var appointment = new Appointment();

        //Act
        await _appointmentRepository.DeleteAsync(appointment, _cancellationToken);

        //Assert
        _context.Verify(x => x.Remove(appointment), Times.Once());
        _context.Verify(x => x.SaveChangesAsync(_cancellationToken), Times.Once());
    }

    #endregion
}
