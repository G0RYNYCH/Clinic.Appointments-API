using Appointments_API.Models.Dto;
using FluentValidation;

namespace Appointments_API.Models.Validators;

public class AppointmentDtoValidator : AbstractValidator<AppointmentDto>
{
    public AppointmentDtoValidator()
    {
        RuleFor(x => x.PatientId)
            .NotEmpty()
            .Must(x => x != Guid.Empty);

        RuleFor(x => x.DoctorId)
            .NotEmpty()
            .Must(x => x != Guid.Empty);

        RuleFor(x => x.ServiceId)
            .NotEmpty()
            .Must(x => x != Guid.Empty);

        RuleFor(x => x.Date)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now))
            .WithMessage("Appointment date must be greater than current date.");

        RuleFor(x => x.Time)
            .NotEmpty();
    }
}
