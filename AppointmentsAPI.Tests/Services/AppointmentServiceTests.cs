using AppointmentsAPI.Interfaces;
using AppointmentsAPI.Models;
using AppointmentsAPI.Models.Dto;
using AppointmentsAPI.Services;
using AutoMapper;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Appointments_API.Tests.Services;

public class AppointmentServiceTests
{
    private readonly AppointmentService _appointmentService;
    private readonly Mock<IAppointmentRepository> _appointmentRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IHttpClientFactory> _httpClientFactory;
    private readonly CancellationToken _cancellationToken;

    public AppointmentServiceTests()
    {
        _appointmentRepository = new Mock<IAppointmentRepository>();
        _mapper = new Mock<IMapper>();
        _cancellationToken = new CancellationToken();
        _appointmentService = new AppointmentService(_appointmentRepository.Object, _mapper.Object, _httpClientFactory.Object);
    }

    #region Ctor 

    [Fact]
    public void AppointmentServiceCtor_NullAppointmentRepository_ThrowsNullArgumentException()
    {
        //Arrange
        //Act
        //Assert
        Assert.Throws<ArgumentNullException>(() => new AppointmentService(null, _mapper.Object, _httpClientFactory.Object));
    }

    [Fact]
    public void AppointmentServiceCtor_NullMapper_ThrowsNullArgumentException()
    {
        //Arrange
        //Act
        //Assert
        Assert.Throws<ArgumentNullException>(() => new AppointmentService(_appointmentRepository.Object, null, _httpClientFactory.Object));
    }
    
    [Fact]
    public void AppointmentServiceCtor_NullHttpClient_ThrowsNullArgumentException()
    {
        //Arrange
        //Act
        //Assert
        Assert.Throws<ArgumentNullException>(() => new AppointmentService(_appointmentRepository.Object, _mapper.Object, null));
    }

    [Fact]
    public void AppointmentServiceCtor_WithNotNullArgs_CreatesInstance()
    {
        //Arrange
        //Act
        var result = new AppointmentService(_appointmentRepository.Object, _mapper.Object, _httpClientFactory.Object);

        //Assert
        result.Should().NotBeNull();
    }

    #endregion

    #region SearchAsync

    [Fact]
    public async Task SearchAsync_ValidParams_Succeeded()
    {
        //Arrange
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

        _appointmentRepository.Setup(x => x.SearchAsync(searchDto, _cancellationToken))
            .ReturnsAsync(searchResult);

        //Act
        var result = await _appointmentService.SearchAsync(searchDto, _cancellationToken);

        //Assert
        _appointmentRepository.Verify(x => x.SearchAsync(searchDto, _cancellationToken), Times.Once());
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(1);
    }

    #endregion

    #region GetAllAsync

    [Fact]
    public async Task GetAllAsync_ValidParams_Succeeded()
    {
        //Arrange
        var searchResult = new List<Appointment>()
        {
            new Appointment(),
            new Appointment(),
            new Appointment(),
        };

        _appointmentRepository.Setup(x => x.GetAllAsync(_cancellationToken))
            .ReturnsAsync(searchResult);

        //Act
        var result = await _appointmentService.GetAllAsync(_cancellationToken);

        //Assert
        _appointmentRepository.Verify(x => x.GetAllAsync(_cancellationToken), Times.Once());
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

        _appointmentRepository.Setup(x => x.GetByIdAsync(appointmentId, _cancellationToken))
            .ReturnsAsync(searchResult);

        //Act
        var result = await _appointmentService.GetByIdAsync(appointmentId, _cancellationToken);

        //Assert
        _appointmentRepository.Verify(x => x.GetByIdAsync(appointmentId, _cancellationToken), Times.Once());
        result.Should().NotBeNull();
        result.Id.Should().Be(searchResult.Id);
    }

    #endregion

    #region CreateAsync

    [Fact]
    public async Task CreateAsync_ValidParams_Succeeded()
    {
        //Arrange
        var appointmentDto = new AppointmentDto();
        var appointment = new Appointment();

        _mapper.Setup(x => x.Map<AppointmentDto, Appointment>(appointmentDto))
            .Returns(appointment);

        //Act
        await _appointmentService.CreateAsync(appointmentDto, _cancellationToken);

        //Assert
        _mapper.Verify(x => x.Map<AppointmentDto, Appointment>(appointmentDto), Times.Once());
        _appointmentRepository.Verify(x => x.CreateAsync(appointment, _cancellationToken), Times.Once());
    }

    #endregion

    #region UpdateAsync

    [Fact]
    public async Task UpdateAsync_ValidParams_Succeeded()
    {
        //Arrange
        var appointmentId = Guid.NewGuid();
        var appointment = new Appointment()
        {
            Id = appointmentId,
        };
        var updateAppointmentDto = new UpdateAppointmentDto();

        _mapper.Setup(x => x.Map<UpdateAppointmentDto, Appointment>(updateAppointmentDto))
            .Returns(appointment);

        //Act
        await _appointmentService.UpdateAsync(appointmentId, updateAppointmentDto, _cancellationToken);

        //Assert
        _appointmentRepository.Verify(x => x.GetByIdAsync(appointmentId, _cancellationToken));
        _mapper.Verify(x => x.Map<UpdateAppointmentDto, Appointment>(updateAppointmentDto), Times.Once());
        _appointmentRepository.Verify(x => x.UpdateAsync(appointment, _cancellationToken), Times.Once());
    }

    #endregion

    #region DeleteAsync

    [Fact]
    public async Task DeleteAsync_ValidParams_Succeeded()
    {
        //Arrange
        var appointmentId = Guid.NewGuid();
        var appointment = new Appointment()
        {
            Id = appointmentId
        };

        _appointmentRepository.Setup(x => x.GetByIdAsync(appointmentId, _cancellationToken))
            .ReturnsAsync(appointment);

        //Act
        await _appointmentService.DeleteAsync(appointmentId, _cancellationToken);

        //Assert
        _appointmentRepository.Verify(x => x.GetByIdAsync(appointmentId, _cancellationToken), Times.Once());
        _appointmentRepository.Verify(x => x.DeleteAsync(appointment, _cancellationToken), Times.Once());
    }

    #endregion
}
