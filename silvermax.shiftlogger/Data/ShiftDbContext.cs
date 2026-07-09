using Microsoft.EntityFrameworkCore;
using silvermax.shiftlogger.Models;

namespace silvermax.shiftlogger.Data;

public class ShiftDbContext : DbContext
{
    public ShiftDbContext(DbContextOptions options) : base(options)
    {
        
    }

    public DbSet<Shift> Shifts => Set<Shift>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Shift>(entity =>
        {
            entity.Property(e => e.ShiftId)
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .ValueGeneratedOnAdd();
            entity.Property(e => e.Duration)
                .HasConversion(
                    v => (long)v.TotalSeconds,
                    v => TimeSpan.FromSeconds(v))
                .HasColumnType("bigint")
                .HasComputedColumnSql("DATEDIFF_BIG(SECOND, StartTime, EndTime)", stored: true);
        });
    }
}
