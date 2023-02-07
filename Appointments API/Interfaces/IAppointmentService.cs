using Appointments_API.Models;
using Appointments_API.Models.Dto;

namespace Appointments_API.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<Appointment>> SearchAsync(SearchDto searchDto, CancellationToken cancellationToken);

    Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken);

    Task<Appointment> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task CreateAsync(AppointmentDto entity, CancellationToken cancellationToken);

    Task UpdateAsync(Guid id, UpdateAppointmentDto updateAppointmentDto, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
