using AppointmentsAPI.Models;
using AppointmentsAPI.Models.Dto;
using AutoMapper;

namespace AppointmentsAPI.Models.Mapping;

public class UpdateAppointmentProfile : Profile
{
    public UpdateAppointmentProfile()
    {
        CreateMap<UpdateAppointmentDto, Appointment>();
        CreateMap<Appointment, UpdateAppointmentDto>();
    }
}
