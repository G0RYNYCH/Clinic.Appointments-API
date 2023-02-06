using Appointments_API.Data.Intefaces;
using Appointments_API.Models;
using Appointments_API.Models.Dto;
using Appointments_API.Services.Interfaces;
using AutoMapper;
using System.Linq.Expressions;

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

    public IQueryable<Appointment> GetAll()
    {
        var appointments = _appointmentRepository.GetAll();

        return appointments;
    }

    //public async Task<List<AppointmentDto>> FindAllByConditionAsync(Expression<Func<Appointment, bool>> expression, CancellationToken cancellationToken)
    //{
    //    var appointments = await _appointmentRepository.FindAllByConditionAsync<Appointment>(expression, cancellationToken);

    //    return appointments;
    //}

    public async Task<Appointment> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _appointmentRepository.GetByIdAsync(id, cancellationToken);
    }

    public async Task CreateAsync(AppointmentDto appointmentDto, CancellationToken cancellationToken)
    {
        var appointment = _mapper.Map<AppointmentDto, Appointment>(appointmentDto);
        await _appointmentRepository.CreateAsync(appointment, cancellationToken);
    }

    public async Task UpdateAsync(Guid id, UpdateAppointmentDto updateAppointmentDto, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentRepository.GetByIdAsync(id, cancellationToken);
        try
        {
            appointment = _mapper.Map<UpdateAppointmentDto, Appointment>(updateAppointmentDto);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
