using Microsoft.EntityFrameworkCore;
using ShiftsLogger.Model;

namespace ShiftsLogger.Data;

public class ShiftsLoggerContextDB : DbContext
{
    public ShiftsLoggerContextDB(DbContextOptions options) : base(options)
    {

    }

    public DbSet<Shift> WorkerShifts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shift>().HasData(
    new Shift
    {
        ShiftId = 1,
        WorkerName = "Juan Pérez",
        Date = new DateOnly(2026, 7, 1),
        StartTime = new TimeOnly(6, 0),
        EndTime = new TimeOnly(14, 0),
        HoursWorked = 8.0m,
        ShiftTime = "Morning",
        CreatedAt = new DateTime(2026, 7, 1, 5, 50, 0),
        UpdateAt = new DateTime(2026, 7, 1, 14, 5, 0)
    },
    new Shift
    {
        ShiftId = 2,
        WorkerName = "María Gómez",
        Date = new DateOnly(2026, 7, 1),
        StartTime = new TimeOnly(14, 0),
        EndTime = new TimeOnly(22, 0),
        HoursWorked = 8.0m,
        ShiftTime = "Afternoon",
        CreatedAt = new DateTime(2026, 7, 1, 13, 50, 0),
        UpdateAt = new DateTime(2026, 7, 1, 22, 5, 0)
    },
    new Shift
    {
        ShiftId = 3,
        WorkerName = "Carlos Rodríguez",
        Date = new DateOnly(2026, 7, 2),
        StartTime = new TimeOnly(22, 0),
        EndTime = new TimeOnly(6, 0),
        HoursWorked = 8.0m,
        ShiftTime = "Night",
        CreatedAt = new DateTime(2026, 7, 2, 21, 50, 0),
        UpdateAt = new DateTime(2026, 7, 3, 6, 5, 0)
    },
    new Shift
    {
        ShiftId = 4,
        WorkerName = "Ana Martínez",
        Date = new DateOnly(2026, 7, 2),
        StartTime = new TimeOnly(7, 0),
        EndTime = new TimeOnly(15, 0),
        HoursWorked = 8.0m,
        ShiftTime = "Morning",
        CreatedAt = new DateTime(2026, 7, 2, 6, 50, 0),
        UpdateAt = new DateTime(2026, 7, 2, 15, 5, 0)
    },
    new Shift
    {
        ShiftId = 5,
        WorkerName = "Luis Fernández",
        Date = new DateOnly(2026, 7, 3),
        StartTime = new TimeOnly(8, 0),
        EndTime = new TimeOnly(16, 0),
        HoursWorked = 8.0m,
        ShiftTime = "Morning",
        CreatedAt = new DateTime(2026, 7, 3, 7, 50, 0),
        UpdateAt = new DateTime(2026, 7, 3, 16, 5, 0)
    },
    new Shift
    {
        ShiftId = 6,
        WorkerName = "Laura Sánchez",
        Date = new DateOnly(2026, 7, 3),
        StartTime = new TimeOnly(15, 0),
        EndTime = new TimeOnly(23, 0),
        HoursWorked = 8.0m,
        ShiftTime = "Afternoon",
        CreatedAt = new DateTime(2026, 7, 3, 14, 50, 0),
        UpdateAt = new DateTime(2026, 7, 3, 23, 5, 0)
    },
    new Shift
    {
        ShiftId = 7,
        WorkerName = "Pedro Ramírez",
        Date = new DateOnly(2026, 7, 4),
        StartTime = new TimeOnly(6, 30),
        EndTime = new TimeOnly(14, 30),
        HoursWorked = 8.0m,
        ShiftTime = "Morning",
        CreatedAt = new DateTime(2026, 7, 4, 6, 20, 0),
        UpdateAt = new DateTime(2026, 7, 4, 14, 35, 0)
    },
    new Shift
    {
        ShiftId = 8,
        WorkerName = "Sofía Torres",
        Date = new DateOnly(2026, 7, 4),
        StartTime = new TimeOnly(13, 30),
        EndTime = new TimeOnly(21, 30),
        HoursWorked = 8.0m,
        ShiftTime = "Afternoon",
        CreatedAt = new DateTime(2026, 7, 4, 13, 20, 0),
        UpdateAt = new DateTime(2026, 7, 4, 21, 35, 0)
    },
    new Shift
    {
        ShiftId = 9,
        WorkerName = "Diego Morales",
        Date = new DateOnly(2026, 7, 5),
        StartTime = new TimeOnly(22, 0),
        EndTime = new TimeOnly(6, 0),
        HoursWorked = 8.0m,
        ShiftTime = "Night",
        CreatedAt = new DateTime(2026, 7, 5, 21, 50, 0),
        UpdateAt = new DateTime(2026, 7, 6, 6, 5, 0)
    },
    new Shift
    {
        ShiftId = 10,
        WorkerName = "Camila Herrera",
        Date = new DateOnly(2026, 7, 5),
        StartTime = new TimeOnly(9, 0),
        EndTime = new TimeOnly(17, 0),
        HoursWorked = 8.0m,
        ShiftTime = "Morning",
        CreatedAt = new DateTime(2026, 7, 5, 8, 50, 0),
        UpdateAt = new DateTime(2026, 7, 5, 17, 5, 0)
    }
);
    }
}
