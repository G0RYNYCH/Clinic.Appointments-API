using AppointmentsAPI_DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentsAPI_DAL.Data;

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
