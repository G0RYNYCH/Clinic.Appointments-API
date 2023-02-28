using System;
using System.Threading.Tasks;
using AppointmentsAPI.Models.Dto;
using AppointmentsAPI.Models.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Appointments_API.Tests.Models.Validators;

public class AppointmentDtoValidatorTests
{
    private readonly AppointmentDtoValidator _validator;

    public AppointmentDtoValidatorTests()
    {
        _validator = new AppointmentDtoValidator();
    }

    [Fact]
    public async Task AppointmentDtoValidator_ValidModel_Succeeded()
    {
        //Arrange
        var appointmentDto = new AppointmentDto()
        {
            PatientId = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            Date = DateOnly.MaxValue,
            Time = TimeOnly.MaxValue,
            IsApproved = false
        };

        //Act
        var result = await _validator.TestValidateAsync(appointmentDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PatientId);
        result.ShouldNotHaveValidationErrorFor(x => x.DoctorId);
        result.ShouldNotHaveValidationErrorFor(x => x.ServiceId);
        result.ShouldNotHaveValidationErrorFor(x => x.Date);
        result.ShouldNotHaveValidationErrorFor(x => x.Time);
        result.ShouldNotHaveValidationErrorFor(x => x.IsApproved);
    }
    
    [Fact]
    public async Task AppointmentDtoValidator_InValidDate_ReturnsError()
    {
        //Arrange
        var appointmentDto = new AppointmentDto()
        {
            PatientId = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            Date = DateOnly.MinValue,
            Time = TimeOnly.MaxValue,
            IsApproved = false
        };
    
        //Act
        var result = await _validator.TestValidateAsync(appointmentDto);
    
        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Date)
            .Only()
            .WithErrorMessage("Appointment date must be greater than current date");
    }
}