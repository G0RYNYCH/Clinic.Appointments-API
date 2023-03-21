using System.Text.Json;
using AppointmentsAPI.Interfaces;
using AppointmentsAPI.Models;
using AppointmentsAPI.Models.Dto;
using AutoMapper;

namespace AppointmentsAPI.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IMapper _mapper;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly JsonSerializerOptions _options;

    public AppointmentService(
        IAppointmentRepository appointmentRepository, 
        IMapper mapper, 
        IHttpClientFactory httpClientFactory)
    {
        _appointmentRepository = appointmentRepository ?? throw new ArgumentNullException(nameof(appointmentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        _options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};
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
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.BaseAddress = new Uri("https://localhost:7116/api/");//remember to put backslash at the end of the Uri
       
        using var response = await httpClient.GetAsync($"Doctors/GetById/{appointmentDto.DoctorId}", HttpCompletionOption.ResponseHeadersRead, cancellationToken);
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var doctor = await JsonSerializer.DeserializeAsync<object>(stream, _options, cancellationToken);
        
        var appointment = _mapper.Map<AppointmentDto, Appointment>(appointmentDto);
        //TODO: generate id
        
        await _appointmentRepository.CreateAsync(appointment, cancellationToken);
        
        //add httpClient
        //provide data validation
        //crate requestModel
        //error mess, isSuccess, data?
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
