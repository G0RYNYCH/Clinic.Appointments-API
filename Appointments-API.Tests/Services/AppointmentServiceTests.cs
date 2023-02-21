using Appointments_API.Interfaces;
using Appointments_API.Models;
using Appointments_API.Models.Dto;
using Appointments_API.Services;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Appointments_API.Tests.Services;

public class AppointmentServiceTests
{
    private readonly AppointmentService _appointmentService;
    private readonly Mock<IAppointmentRepository> _appointmentRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly CancellationToken _cancelationToken;

    public AppointmentServiceTests()
    {
        _appointmentRepository = new Mock<IAppointmentRepository>();
        _mapper = new Mock<IMapper>();

        var mockHttpContext = new DefaultHttpContext();//TODO: ? 
        mockHttpContext.RequestAborted = _cancelationToken;

        _appointmentService = new AppointmentService(_appointmentRepository.Object, _mapper.Object);
    }
    //TODO: test ctor or not? 
    #region Ctor 

    [Fact]
    public void AppointmentServiceCtor_NullAppointmentRepository_ThrowsNullArgumentExceeption()
    {
        //Arrenge
        //Act
        //Assert
        Assert.Throws<ArgumentNullException>(() => new AppointmentService(null, _mapper.Object));
    }

    [Fact]
    public void AppointmentServiceCtor_NullMapper_ThrowsNullArgumentException()
    {
        //Arrenge
        //Act
        //Assert
        Assert.Throws<ArgumentNullException>(() => new AppointmentService(_appointmentRepository.Object, null));
    }

    [Fact]
    public void AppointmentServiceCtor_WithNotNullArgs_CreatesInstance()
    {
        //Arrenge
        //Act
        var result = new AppointmentService(_appointmentRepository.Object, _mapper.Object);

        //Assert
        result.Should().NotBeNull();
    }

    #endregion

    #region SearchAsync

    [Fact]
    public async Task SearchAsync_ValidParams_Succeeded()
    {
        //Arrenge
        var searchDto = new SearchDto()
        {
            PageSize = 1,
            PageNumber = 1,
        };

        var testAppointment = new Appointment();

        var searchResult = new List<Appointment>()
        {
            testAppointment
        };

        _appointmentRepository.Setup(x => x.SearchAsync(searchDto, _cancelationToken))
            .ReturnsAsync(searchResult);

        //Act
        var result = await _appointmentService.SearchAsync(searchDto, _cancelationToken);

        //Assert
        _appointmentRepository.Verify(x => x.SearchAsync(searchDto, _cancelationToken), Times.Once());
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(1);
    }

    #endregion

    #region GetAllAsync

    [Fact]
    public async Task GetAllAsync_ValidParams_Succeeded()
    {
        //Arrenge
        var searchResult = new List<Appointment>()
        {
            new Appointment(),
            new Appointment(),
            new Appointment(),
        };

        _appointmentRepository.Setup(x => x.GetAllAsync(_cancelationToken))
            .ReturnsAsync(searchResult);

        //Act
        var result = await _appointmentService.GetAllAsync(_cancelationToken);

        //Assert
        _appointmentRepository.Verify(x => x.GetAllAsync(_cancelationToken), Times.Once());
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(3);
    }

    #endregion

    #region GetByIdAsync

    [Fact]
    public async Task GetByIdAsync_ValidParams_Succeeded()
    {
        // Arrange
        var appointmentId = Guid.NewGuid();
        var searchResult = new Appointment()
        {
            Id = appointmentId,
        };

        _appointmentRepository.Setup(x => x.GetByIdAsync(appointmentId, _cancelationToken))
            .ReturnsAsync(searchResult);

        //Act
        var result = await _appointmentService.GetByIdAsync(appointmentId, _cancelationToken);

        //Assert
        _appointmentRepository.Verify(x => x.GetByIdAsync(appointmentId, _cancelationToken), Times.Once());
        result.Should().NotBeNull();
        result.Id.Should().Be(searchResult.Id);
    }

    #endregion

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_ValidParams_Succeeded()
    {
        //Arrenge
        var appointmentDto = new AppointmentDto();
        var appointment = new Appointment();

        _mapper.Setup(x => x.Map<AppointmentDto, Appointment>(appointmentDto))
            .Returns(appointment);

        //Act
        await _appointmentService.CreateAsync(appointmentDto, _cancelationToken);

        //Assert
        _mapper.Verify(x => x.Map<AppointmentDto, Appointment>(appointmentDto), Times.Once());
        _appointmentRepository.Verify(x => x.CreateAsync(appointment, _cancelationToken), Times.Once());
    }

    #endregion

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_ValidParams_Succeeded()
    {
        //Arrenge
        var appointmentId = Guid.NewGuid();
        var appointment = new Appointment()
        {
            Id = appointmentId,
        };
        var updateAppointmentDto = new UpdateAppointmentDto();

        _mapper.Setup(x => x.Map<UpdateAppointmentDto, Appointment>(updateAppointmentDto))
            .Returns(appointment);

        //Act
        await _appointmentService.UpdateAsync(appointmentId, updateAppointmentDto, _cancelationToken);

        //Assert
        _appointmentRepository.Verify(x => x.GetByIdAsync(appointmentId, _cancelationToken));
        _mapper.Verify(x => x.Map<UpdateAppointmentDto, Appointment>(updateAppointmentDto), Times.Once());
        _appointmentRepository.Verify(x => x.UpdateAsync(appointment, _cancelationToken), Times.Once());
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_ValidParams_Succeeded()
    {
        //Arrenge
        var appointmentId = Guid.NewGuid();
        var appointment = new Appointment()
        {
            Id = appointmentId
        };

        _appointmentRepository.Setup(x => x.GetByIdAsync(appointmentId, _cancelationToken))
            .ReturnsAsync(appointment);

        //Act
        await _appointmentService.DeleteAsync(appointmentId, _cancelationToken);

        //Assert
        _appointmentRepository.Verify(x => x.GetByIdAsync(appointmentId, _cancelationToken), Times.Once());
        _appointmentRepository.Verify(x => x.DeleteAsync(appointment, _cancelationToken), Times.Once());
    }

    #endregion
}
