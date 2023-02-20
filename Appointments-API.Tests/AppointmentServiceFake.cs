using Appointments_API.Interfaces;
using Appointments_API.Models;
using Appointments_API.Models.Dto;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Appointments_API.Tests;

public class AppointmentServiceFake : IAppointmentService
{
    private readonly List<Appointment> _appointments;

    public AppointmentServiceFake()
    {
        _appointments = new List<Appointment>()
        {
            new Appointment()
            {
                Id = new Guid("F4FC7E41-D839-4F06-BC49-00EB9B5024B9"),
                PatientId = new Guid("F7138BBE-4D79-4D45-84AF-E7D26FBEA001"),
                DoctorId = new Guid("F1EB30F1-6CDD-41E8-A76B-E5C39720AEFE"),
                ServiceId = new Guid("0F0DCF83-2E7B-4C32-BE06-8A70C37290F0"),
                Date = DateOnly.FromDateTime(DateTime.Now).AddYears(1),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                IsApproved = false
            },
            new Appointment()
            {
                Id = new Guid("F4FC7E41-D839-4F06-BC49-00EB9B5024B9"),
                PatientId = new Guid("025694B2-550A-4C75-884A-F97E993CEDB6"),
                DoctorId = new Guid("60561F1E-B0EE-4957-9E35-E21B58DADA88"),
                ServiceId = new Guid("3D410843-7853-4013-9383-E637F8F7EE6F"),
                Date = DateOnly.FromDateTime(DateTime.Now).AddYears(1),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                IsApproved = false
            },
            new Appointment()
            {
                Id = new Guid("F4FC7E41-D839-4F06-BC49-00EB9B5024B9"),
                PatientId = new Guid("05197816-923D-4036-AB51-452AEBA61E43"),
                DoctorId = new Guid("A5AF74D2-0860-41AA-A8F2-5B243DA02812"),
                ServiceId = new Guid("39CFACDD-F3F5-43AD-90C4-DD5AAFA752CF"),
                Date = DateOnly.FromDateTime(DateTime.Now).AddYears(1),
                Time = TimeOnly.FromDateTime(DateTime.Now),
                IsApproved = false
            },
        };
    }

    public Task<IEnumerable<Appointment>> SearchAsync(SearchDto searchDto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Appointment> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task CreateAsync(AppointmentDto entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Guid id, UpdateAppointmentDto updateAppointmentDto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
