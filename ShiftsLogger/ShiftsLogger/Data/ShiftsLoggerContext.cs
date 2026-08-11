using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Models;

namespace ShiftsLogger.Data;

public class ShiftsLoggerContext : DbContext
{
    public ShiftsLoggerContext(DbContextOptions<ShiftsLoggerContext> options) : base(options)
    {
    }

    public DbSet<Worker> Workers { get; set; } = null!;
    public DbSet<Shift> Shifts { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shift>()
            .HasOne(s => s.Worker)
            .WithMany(w => w.Shifts)
            .HasForeignKey(s => s.WorkerId);

        modelBuilder.Entity<Shift>()
            .Ignore(s => s.Duration);
    }
}
