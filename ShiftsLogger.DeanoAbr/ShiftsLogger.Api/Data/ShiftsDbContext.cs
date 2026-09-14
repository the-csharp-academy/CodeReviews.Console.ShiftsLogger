using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Api.Models;

namespace ShiftsLogger.Api.Data;

public class ShiftsDbContext : DbContext
{
    public ShiftsDbContext(DbContextOptions<ShiftsDbContext> options) : base(options) { }

    public DbSet<Shift> Shifts => Set<Shift>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var shift = modelBuilder.Entity<Shift>();

        shift.Property(s => s.WorkerName)
             .IsRequired()
             .HasMaxLength(100);

        shift.HasIndex(s => s.WorkerName);
    }
}
