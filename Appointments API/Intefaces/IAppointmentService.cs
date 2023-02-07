using Appointments_API.Models;
using Appointments_API.Models.Dto;

namespace Appointments_API.Interfaces;

public interface IAppointmentService
{
    Task<IEnumerable<Appointment>> Search(SearchDto searchDto);

    Task<IEnumerable<Appointment>> GetAll();

    Task<Appointment> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task CreateAsync(AppointmentDto entity, CancellationToken cancellationToken);

    Task UpdateAsync(Guid id, UpdateAppointmentDto updateAppointmentDto, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}
