using Microsoft.EntityFrameworkCore;
using ShiftsLoggerApi.Models;

namespace ShiftsLoggerApi.Data
{
    public class ShiftContext : DbContext
    {
        public DbSet<ShiftsLog> Shifts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=localhost;Initial Catalog=ShiftsLogger;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Application Name=""SQL Server Management Studio"";Command Timeout=0");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShiftsLog>().HasData(
                new ShiftsLog { Id = 1, StartDate = new DateTime(2026, 1, 2), EndDate = new DateTime(2026, 1, 3), WorkerId = 7 },
                new ShiftsLog { Id = 2, StartDate = new DateTime(2026, 1, 5), EndDate = new DateTime(2026, 1, 6), WorkerId = 3 });
        }
    }
}