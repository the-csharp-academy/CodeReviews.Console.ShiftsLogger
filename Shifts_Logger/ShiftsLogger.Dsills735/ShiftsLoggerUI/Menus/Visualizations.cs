using ShiftsLoggerUI.Models;
using Spectre.Console;

namespace ShiftsLoggerUI.Menus
{
    internal class Visualizations
    {
        internal static void ShowAllShiftsTable(List<Shift> shifts, bool stay = true)
        {
            var table = new Table()
                .RoundedBorder()
                .BorderColor(Color.Blue)
                .Title("Shifts")
                .AddColumn("Shift ID")
                .AddColumn("Job Title")
                .AddColumn("Start Time")
                .AddColumn("End Time")
                .AddColumn("Hours Worked");
            
            foreach (var shift in shifts)
            {
     
                table.AddRow(shift.id.ToString(), shift.JobTitle, shift.start.ToString("yyyy-MM-dd HH:mm"), shift.end.ToString("yyyy-MM-dd HH:mm"), shift.HoursWorked.ToString("F2"));
            }

            AnsiConsole.Write(table);

            if (stay)
            {
                Console.WriteLine("Press any key to return to the menu");
                Console.ReadKey();
            }
        }
    }
}
