using Appointments_API.Data.Interfaces;
using Appointments_API.Models;

namespace Appointments_API.Data.Repositories;

public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
{
    public AppointmentRepository(AppointmentDbContext dbContext) : base(dbContext)
    {

    }
}
