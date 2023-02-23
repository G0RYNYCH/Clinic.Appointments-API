using AppointmentsAPI.Interfaces;
using AppointmentsAPI.Models;
using AppointmentsAPI.Models.Dto;
using AutoMapper;

namespace AppointmentsAPI.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;

    public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
    {
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public Task<IEnumerable<Appointment>> SearchAsync(SearchDto searchDto, CancellationToken cancellationToken)
    {
        return _appointmentRepository.SearchAsync(searchDto, cancellationToken);
    }

    public Task<IEnumerable<Appointment>> GetAllAsync(CancellationToken cancellationToken)
    {
        return _appointmentRepository.GetAllAsync(cancellationToken);
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
        appointment = _mapper.Map<UpdateAppointmentDto, Appointment>(updateAppointmentDto);
        await _appointmentRepository.UpdateAsync(appointment, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);
        await _appointmentRepository.DeleteAsync(appointment, cancellationToken);
    }
}
