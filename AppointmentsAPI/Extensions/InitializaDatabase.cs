using AppointmentsAPI.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AppointmentsAPI.Extensions;

public static class InitializeDatabase
{
    public static void Initialize()
    {
        using var dbContext = new AppointmentDbContext(new DbContextOptions<AppointmentDbContext>());
        dbContext.Database.Migrate();
    }
}