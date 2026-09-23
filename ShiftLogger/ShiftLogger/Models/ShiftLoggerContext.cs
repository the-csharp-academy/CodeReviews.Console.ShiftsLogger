using Microsoft.EntityFrameworkCore;

namespace ShiftLogger;

public class ShiftLoggerContext : DbContext
{
    public DbSet<Worker> Workers { get; set; }
    public DbSet<Shift> Shifts { get; set; }

    public ShiftLoggerContext(DbContextOptions<ShiftLoggerContext> options) : base(options)
    {     
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Worker>()
        .HasIndex(w => w.Id)
        .IsUnique();
    }
}