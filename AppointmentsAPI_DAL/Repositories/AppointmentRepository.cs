using AppointmentsAPI_DAL.Data;
using AppointmentsAPI_DAL.Interfaces;
using AppointmentsAPI_DAL.Models;

namespace AppointmentsAPI_DAL.Repositories;

public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(AppointmentDbContext dbContext) : base(dbContext)
    {

    }
}
