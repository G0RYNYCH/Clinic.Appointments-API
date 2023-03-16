using AppointmentsAPI.Interfaces;
using AppointmentsAPI.Models;
using AppointmentsAPI.Models.Dto;
using AutoMapper;

namespace AppointmentsAPI.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;
    private static readonly HttpClient _httpClient = new HttpClient();

    public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
    {
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _httpClient.BaseAddress = new Uri("http://localhost:5116"); //TODO: docker service uri
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
        //add httpClient
        //provide data validation
        //crate requestModel
        //error mess, isSuccess, data?
        
        var response = await _httpClient.GetAsync("api/profiles", cancellationToken);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        Console.WriteLine(content);
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
