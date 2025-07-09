using Microsoft.EntityFrameworkCore;
using Shifts_Logger.Models;

namespace Shifts_Logger.Data;

public class ShiftsDbContext : DbContext
{
    public ShiftsDbContext(DbContextOptions<ShiftsDbContext> options)
        : base(options) { }

    public DbSet<Shift> Shifts { get; set; }

    public void SeedData()
    {
        Shifts.RemoveRange(Shifts);
        Shifts.AddRange(
            new Shift
            {
                EmployeeName = "Guinna Marsden",
                Position = "Editor",
                ShiftNumber = 1,
                StartTime = new DateTime(2025, 7, 6, 0, 0, 0),
                EndTime = new DateTime(2025, 7, 6, 8, 12, 0),
            },
            new Shift
            {
                EmployeeName = "Dela Kingescot",
                Position = "Desktop Support Technician",
                ShiftNumber = 1,
                StartTime = new DateTime(2025, 7, 6, 0, 0, 0),
                EndTime = new DateTime(2025, 7, 6, 8, 15, 0),
            },
            new Shift
            {
                EmployeeName = "Alejoa Boddice",
                Position = "Marketing Assistant",
                ShiftNumber = 1,
                StartTime = new DateTime(2025, 7, 6, 0, 0, 0),
                EndTime = new DateTime(2025, 7, 6, 8, 7, 0),
            },
            new Shift
            {
                EmployeeName = "Oralia Stollwerck",
                Position = "Information Systems Manager",
                ShiftNumber = 1,
                StartTime = new DateTime(2025, 7, 6, 8, 1, 0),
                EndTime = new DateTime(2025, 7, 6, 16, 5, 0),
            },
            new Shift
            {
                EmployeeName = "Wendeline Odhams",
                Position = "Engineer IV",
                ShiftNumber = 2,
                StartTime = new DateTime(2025, 7, 6, 8, 0, 0),
                EndTime = new DateTime(2025, 7, 6, 16, 0, 0),
            },
            new Shift
            {
                EmployeeName = "Octavius Pickthall",
                Position = "Software Consultant",
                ShiftNumber = 2,
                StartTime = new DateTime(2025, 7, 6, 8, 0, 0),
                EndTime = new DateTime(2025, 7, 6, 16, 5, 0),
            },
            new Shift
            {
                EmployeeName = "Kassie Benediktsson",
                Position = "Social Worker",
                ShiftNumber = 2,
                StartTime = new DateTime(2025, 7, 6, 8, 0, 0),
                EndTime = new DateTime(2025, 7, 6, 16, 0, 0),
            },
            new Shift
            {
                EmployeeName = "Cynthie McTrusty",
                Position = "Civil Engineer",
                ShiftNumber = 3,
                StartTime = new DateTime(2025, 7, 6, 16, 0, 0),
                EndTime = new DateTime(2025, 7, 7, 0, 0, 0),
            },
            new Shift
            {
                EmployeeName = "Constantine Northfield",
                Position = "Electrical Engineer",
                ShiftNumber = 3,
                StartTime = new DateTime(2025, 7, 6, 15, 59, 0),
                EndTime = new DateTime(2025, 7, 7, 0, 0, 0),
            },
            new Shift
            {
                EmployeeName = "Spenser Dunmore",
                Position = "Analog Circuit Design Manager",
                ShiftNumber = 3,
                StartTime = new DateTime(2025, 7, 6, 16, 1, 0),
                EndTime = new DateTime(2025, 7, 7, 0, 5, 0),
            }
        );

        SaveChanges();
    }
}
