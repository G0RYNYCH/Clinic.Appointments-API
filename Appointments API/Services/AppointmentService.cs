using Appointments_API.Interfaces;
using Appointments_API.Models;
using Appointments_API.Models.Dto;
using AutoMapper;

namespace Appointments_API.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;

    public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
    {
        _appointmentRepository = appointmentRepository;
        _mapper = mapper;
    }

    public Task<IEnumerable<Appointment>> SearchAsync(SearchDto searchDto, CancellationToken cancellationToken)
    {
        var appointments = _appointmentRepository.SearchAsync(searchDto, cancellationToken);

        return appointments;
    }

    public Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken)
    {
        var appointments = _appointmentRepository.GetAllAsync(cancellationToken);

        return appointments;
    }

    public async Task<Appointment> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _appointmentRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task CreateAsync(AppointmentDto appointmentDto, CancellationToken cancellationToken)
    {
        var appointment = _mapper.Map<AppointmentDto, Appointment>(appointmentDto);
        await _appointmentRepository.CreateAsync(appointment, cancellationToken);
        // add httpClient
    }

    public async Task UpdateAsync(Guid id, UpdateAppointmentDto updateAppointmentDto, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);
        try
        {
            appointment = _mapper.Map<UpdateAppointmentDto, Appointment>(updateAppointmentDto);
            await _appointmentRepository.UpdateAsync(appointment, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);
        try
        {
            await _appointmentRepository.DeleteAsync(appointment, cancellationToken);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
