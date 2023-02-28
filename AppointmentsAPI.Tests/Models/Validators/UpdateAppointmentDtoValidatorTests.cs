using System;
using System.Threading.Tasks;
using AppointmentsAPI.Models.Dto;
using AppointmentsAPI.Models.Validators;
using FluentValidation.TestHelper;
using Xunit;

namespace Appointments_API.Tests.Models.Validators;

public class UpdateAppointmentDtoValidatorTests
{
    private readonly UpdateAppointmentDtoValidator _validator;

    public UpdateAppointmentDtoValidatorTests()
    {
        _validator = new UpdateAppointmentDtoValidator();
    }

    [Fact]
    public async Task UpdateAppointmentDtoValidator_ValidModel_Succeeded()
    {
        //Arrange
        var updateAppointmentDto = new UpdateAppointmentDto()
        {
            PatientId = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            Date = DateOnly.MaxValue,
            Time = TimeOnly.MaxValue,
            IsApproved = false
        };

        //Act
        var result = await _validator.TestValidateAsync(updateAppointmentDto);

        //Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PatientId);
        result.ShouldNotHaveValidationErrorFor(x => x.DoctorId);
        result.ShouldNotHaveValidationErrorFor(x => x.ServiceId);
        result.ShouldNotHaveValidationErrorFor(x => x.Date);
        result.ShouldNotHaveValidationErrorFor(x => x.Time);
        result.ShouldNotHaveValidationErrorFor(x => x.IsApproved);
    }
    
    [Fact]
    public async Task UpdateAppointmentDtoValidator_InValidDate_ReturnsError()
    {
        //Arrange
        var updateAppointmentDto = new UpdateAppointmentDto()
        {
            PatientId = Guid.NewGuid(),
            DoctorId = Guid.NewGuid(),
            ServiceId = Guid.NewGuid(),
            Date = DateOnly.MinValue,
            Time = TimeOnly.MaxValue,
            IsApproved = false
        };

        //Act
        var result = await _validator.TestValidateAsync(updateAppointmentDto);

        //Assert
        result.ShouldHaveValidationErrorFor(x => x.Date)
            .Only()
            .WithErrorMessage("Appointment date must be greater than current date");
    }
}