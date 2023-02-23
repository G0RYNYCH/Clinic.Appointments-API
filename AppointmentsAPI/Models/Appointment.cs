using System.Diagnostics.CodeAnalysis;

namespace AppointmentsAPI.Models;

[ExcludeFromCodeCoverage]
public class Appointment : EntityBase
{
    public Guid PatientId { get; set; }

    public Guid DoctorId { get; set; }

    public Guid ServiceId { get; set; }

    public DateOnly Date { get; set; }

    public TimeOnly Time { get; set; }

    public bool IsApproved { get; set; }
}
