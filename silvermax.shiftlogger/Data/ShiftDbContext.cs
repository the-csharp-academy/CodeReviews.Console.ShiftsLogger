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
            entity.Property(e => e.Name).HasMaxLength(100);
        });
    }
}
