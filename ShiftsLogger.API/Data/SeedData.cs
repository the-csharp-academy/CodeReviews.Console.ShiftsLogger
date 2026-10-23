using Microsoft.EntityFrameworkCore;

public static class SeedData
{
    public static void Initialize(DbContext context)
    {
        context.Database.Migrate();


        var rob = new Employee
        {
            FirstName = "Rob",
            LastName = "Truman",
            Role = "Nurse"
        };

        var halina = new Employee
        {
            FirstName = "Halina",
            LastName = "Ostapchuk",
            Role = "Doctor"
        };
        if (!context.Set<Employee>().Any())
        {
            context.Set<Employee>().AddRange(rob, halina);
            context.SaveChanges();
        }

        if (!context.Set<Shift>().Any())
        {
            var shiftRecords = new List<Shift>
            {
                new()
                {
                    Employee = rob,
                    StartTime = new DateTime(2026, 9, 16, 8, 0, 0),
                    EndTime = new DateTime(2026, 9, 16, 16, 0, 0)
                },
                new()
                {
                    Employee = halina,
                    StartTime = new DateTime(2026, 9, 17, 12, 0, 0),
                    EndTime = new DateTime(2026, 9, 17, 20, 0, 0)
                }
            };

            context.Set<Shift>().AddRange(shiftRecords);
            context.SaveChanges();
        }
    }
}