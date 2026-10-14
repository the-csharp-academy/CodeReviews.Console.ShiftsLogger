using Microsoft.EntityFrameworkCore;
using Shifts.WebAPIs.Models;

namespace Shifts.WebAPIs.Data
{
    public class ShiftsDbContext : DbContext
    {
        public DbSet<Shift> Shifts { get; set; }
        public ShiftsDbContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}
