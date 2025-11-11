using Microsoft.EntityFrameworkCore;
using static ShiftLogger.Models.ShiftLoggerEntity.ShifLoggerEntity;

namespace ShiftLogger.Models
{
    public class ShiftLoggerDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ShiftLoggerDbContext(DbContextOptions<ShiftLoggerDbContext> options) : base(options)
        {
            _configuration = new ConfigurationBuilder()
                            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .Build();
        }

        public DbSet<EmployeeList> EmployeeList { get; set; }
        public DbSet<ShiftDetails> ShiftDetails { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShiftDetails>()
           .Property(s => s.ShiftStatus)
           .HasDefaultValue(1);

            modelBuilder.Entity<ShiftDetails>()
           .Property(s => s.CreatedDate)
           .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ShiftDetails>()
            .Property(s => s.ShiftStart)
            .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<ShiftDetails>()
            .HasOne(s => s.Employee)
            .WithMany(e => e.Shifts)
            .HasForeignKey(s => s.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeList>().HasData(
                 new EmployeeList { Id = 1, Name = "Raj", CreatedDate = DateTime.Parse("2025-09-16 05:49:50.880") },
                 new EmployeeList { Id = 2, Name = "Kumar", CreatedDate = DateTime.Parse("2025-09-17 08:23:10.320") },
                 new EmployeeList { Id = 3, Name = "Priya", CreatedDate = DateTime.Parse("2025-09-18 10:11:22.500") },
                 new EmployeeList { Id = 4, Name = "Suresh", CreatedDate = DateTime.Parse("2025-09-19 12:45:35.250") },
                 new EmployeeList { Id = 5, Name = "Anita", CreatedDate = DateTime.Parse("2025-09-20 14:05:45.880") },
                 new EmployeeList { Id = 6, Name = "Manoj", CreatedDate = DateTime.Parse("2025-09-21 16:15:55.123") },
                 new EmployeeList { Id = 7, Name = "Divya", CreatedDate = DateTime.Parse("2025-09-22 18:25:35.600") },
                 new EmployeeList { Id = 8, Name = "Vijay", CreatedDate = DateTime.Parse("2025-09-23 19:30:15.770") },
                 new EmployeeList { Id = 9, Name = "Sneha", CreatedDate = DateTime.Parse("2025-09-24 20:45:55.980") },
                 new EmployeeList { Id = 10, Name = "Arun", CreatedDate = DateTime.Parse("2025-09-25 21:55:05.440") },
                 new EmployeeList { Id = 11, Name = "Lakshmi", CreatedDate = DateTime.Parse("2025-09-26 22:10:30.210") }
             );

            modelBuilder.Entity<ShiftDetails>().HasData(
                 new ShiftDetails { Id = 1028, EmployeeId = 1, ShiftStart = new DateTime(2025, 11, 08, 9, 25, 53), ShiftEnd = new DateTime(2025, 11, 09, 6, 33, 06), TotalWorkingHours = 21.12m, CreatedDate = new DateTime(2025, 11, 08, 9, 25, 55), ShiftStatus = 0 },
                 new ShiftDetails { Id = 1029, EmployeeId = 2, ShiftStart = new DateTime(2025, 11, 07, 8, 15, 00), ShiftEnd = new DateTime(2025, 11, 07, 17, 45, 00), TotalWorkingHours = 9.5m, CreatedDate = new DateTime(2025, 11, 07, 8, 15, 00), ShiftStatus = 1 },
                 new ShiftDetails { Id = 1030, EmployeeId = 3, ShiftStart = new DateTime(2025, 11, 06, 10, 00, 00), ShiftEnd = new DateTime(2025, 11, 06, 20, 15, 00), TotalWorkingHours = 10.25m, CreatedDate = new DateTime(2025, 11, 06, 10, 00, 00), ShiftStatus = 0 },
                 new ShiftDetails { Id = 1031, EmployeeId = 1, ShiftStart = new DateTime(2025, 11, 05, 9, 00, 00), ShiftEnd = new DateTime(2025, 11, 05, 18, 00, 00), TotalWorkingHours = 9.0m, CreatedDate = new DateTime(2025, 11, 05, 9, 00, 00), ShiftStatus = 1 },
                 new ShiftDetails { Id = 1032, EmployeeId = 4, ShiftStart = new DateTime(2025, 11, 04, 7, 45, 00), ShiftEnd = new DateTime(2025, 11, 04, 16, 00, 00), TotalWorkingHours = 8.25m, CreatedDate = new DateTime(2025, 11, 04, 7, 45, 00), ShiftStatus = 0 }
             );

        }
    }
}
