using Microsoft.EntityFrameworkCore;

public class ShiftsLoggerDbContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Shift> Shifts { get; set; }

    public ShiftsLoggerDbContext(DbContextOptions options) : base(options) { }
}