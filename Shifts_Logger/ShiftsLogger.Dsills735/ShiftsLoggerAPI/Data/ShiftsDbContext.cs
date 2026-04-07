using Microsoft.EntityFrameworkCore;
using ShiftsLoggerAPI.Models;

namespace ShiftsLoggerAPI.Data
{
    public class ShiftsDbContext(DbContextOptions<ShiftsDbContext> options) : DbContext(options)
    {
        public Microsoft.EntityFrameworkCore.DbSet<Shift> Shifts { get; set; }
    }
}