using Appointments_API.Interfaces;
using Appointments_API.Models;

namespace Appointments_API.Repositories;

public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(AppointmentDbContext dbContext) : base(dbContext)
    {

    }
}
