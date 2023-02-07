using Appointments_API.Models.Dto;
using AutoMapper;

namespace Appointments_API.Models.Mapping;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<AppointmentDto, Appointment>();
        CreateMap<Appointment, AppointmentDto>();
    }
}
