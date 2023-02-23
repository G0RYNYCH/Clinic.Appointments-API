using AppointmentsAPI.Models.Dto;
using AutoMapper;

namespace AppointmentsAPI.Models.Mapping;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<AppointmentDto, Appointment>();
        CreateMap<Appointment, AppointmentDto>();
    }
}
