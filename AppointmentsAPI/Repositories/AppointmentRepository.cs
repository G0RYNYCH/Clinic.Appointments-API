using AppointmentsAPI.Interfaces;
using AppointmentsAPI.Models;

namespace AppointmentsAPI.Repositories;

public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(AppointmentDbContext dbContext) : base(dbContext)
    {

    }
}
