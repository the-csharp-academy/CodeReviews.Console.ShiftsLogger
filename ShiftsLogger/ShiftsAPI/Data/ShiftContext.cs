using Microsoft.EntityFrameworkCore;
using ClassLibray.Models;

namespace ShiftsAPI.Data;

public class ShiftContext : DbContext
{
    public ShiftContext(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Shift> Shifts { get; set; }
}