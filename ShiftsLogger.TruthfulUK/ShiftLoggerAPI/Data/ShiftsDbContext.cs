using Microsoft.EntityFrameworkCore;
using ShiftLoggerAPI.Models;

namespace ShiftLoggerAPI.Data;

public class ShiftsDbContext : DbContext
{
    public ShiftsDbContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Shift> Shifts { get; set; }
}
