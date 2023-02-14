using Appointments_API.Models.Dto;
using FluentValidation;

namespace Appointments_API.Models.Validators;

public class UpdateAppointmentDtoValidator : AbstractValidator<UpdateAppointmentDto>
{
    public UpdateAppointmentDtoValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty();

        RuleFor(x => x.DoctorId)
            .NotEmpty();

        RuleFor(x => x.ServiceId)
            .NotEmpty();

        RuleFor(x => x.Date)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Appointment date must be greater than current date.");

        RuleFor(x => x.Time)
            .NotEmpty();

        RuleFor(x => x.IsApproved)
            .NotEmpty();
    }
}
