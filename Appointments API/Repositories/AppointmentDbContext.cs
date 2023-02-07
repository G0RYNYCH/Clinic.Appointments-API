using Appointments_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Appointments_API.Repositories;

public class AppointmentDbContext : DbContext
{
    public DbSet<Appointment> Appointments { get; set; }

    public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(x => x.Id);
        });
    }
}
