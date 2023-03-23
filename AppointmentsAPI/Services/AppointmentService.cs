using AppointmentsAPI.Interfaces;
using AppointmentsAPI.Models;
using AppointmentsAPI.Models.Dto;
using AutoMapper;

namespace AppointmentsAPI.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;
    private readonly IHttpClientService _clientService;

    public AppointmentService(
        IAppointmentRepository appointmentRepository, 
        IMapper mapper, 
        IHttpClientService clientService)
    {
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _clientService = clientService ?? throw new ArgumentNullException(nameof(clientService));
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
        //request to the ProfilesAPI
        var isDoctorExists = _clientService.IsDoctorExistsAsync(appointmentDto.DoctorId, cancellationToken).Result;
        if (!isDoctorExists)
        {
            return;//TODO: ?
        }
        var isPatientExists = _clientService.IsPatientExistsAsync(appointmentDto.PatientId, cancellationToken).Result;
        if (!isPatientExists)
        {
            return;//TODO: ?
        }
        
        var appointment = _mapper.Map<AppointmentDto, Appointment>(appointmentDto);
        appointment.Id = Guid.NewGuid();
        await _appointmentRepository.CreateAsync(appointment, cancellationToken);
        
        //add httpClient
        //provide data validation
        //crate requestModel
        //error mess, isSuccess, data?
    }

    public async Task UpdateAsync(Guid id, UpdateAppointmentDto updateAppointmentDto, CancellationToken cancellationToken)
    {
        var isDoctorExists = _clientService.IsDoctorExistsAsync(updateAppointmentDto.DoctorId, cancellationToken).Result;
        if (!isDoctorExists)
        {
            return;//TODO: ?
        }
        var isPatientExists = _clientService.IsPatientExistsAsync(updateAppointmentDto.PatientId, cancellationToken).Result;
        if (!isPatientExists)
        {
            return;//TODO: ?
        }
        
        var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);
        appointment = _mapper.Map<UpdateAppointmentDto, Appointment>(updateAppointmentDto);
        appointment.Id = Guid.NewGuid();
        await _appointmentRepository.UpdateAsync(appointment, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);
        await _appointmentRepository.DeleteAsync(appointment, cancellationToken);
    }
}
