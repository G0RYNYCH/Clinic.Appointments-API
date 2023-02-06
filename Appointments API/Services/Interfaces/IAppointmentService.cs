using Appointments_API.Models;
using Appointments_API.Models.Dto;
using System.Linq.Expressions;

namespace Appointments_API.Services.Interfaces;

public interface IAppointmentService
{
    IQueryable<Appointment> GetAll();
    //Task<List<AppointmentDto>> FindAllByConditionAsync(Expression<Func<Appointment, bool>> expression, CancellationToken cancellationToken);
    Task<Appointment> GetByIdAsync(Guid entity, CancellationToken cancellationToken);
    Task CreateAsync(AppointmentDto entity, CancellationToken cancellationToken);
    Task UpdateAsync(Guid id, UpdateAppointmentDto updateAppointmentDto, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
