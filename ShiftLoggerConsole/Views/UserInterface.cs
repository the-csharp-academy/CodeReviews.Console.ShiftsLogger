using ShiftLoggerConsole.Controllers;

namespace ShiftLoggerConsole.Views;
internal class UserInterface
{
    private ShiftController _shifts = new ShiftController();

    public async Task Display()
    {
        var shifts = await _shifts.GetAllShifts();

        foreach (var s in shifts)
        {
            Console.WriteLine($"{s.Id} {s.Start} {s.End} {s.Duration}");
        }

        Console.ReadKey();
    }
}
