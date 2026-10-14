using Spectre.Console;

namespace Shifts.Logger
{
    public static class UI
    {
        public async static Task PrintTable()
        {
            var table = new Table()
                        .AddColumn("Id")
                        .AddColumn("Employee")
                        .AddColumn("Start time")
                        .AddColumn("End time")
                        .AddColumn("Duaration");

            var shifts = await ShiftsAPI.GetShifts();
            if (shifts == null)
            {
                AnsiConsole.MarkupLine("No shifts yet");
                return;
            }

            foreach (var shift in shifts)
            {
                table.AddRow(
                    shift.Id.ToString(),
                    shift.Employee,
                    shift.StartTime.ToString(),
                    shift.EndTime.ToString(),
                    shift.Duration.ToString());
            }
            AnsiConsole.Write(table);
        }


        public async static Task<bool> CreateShift()
        {
            var employee = AnsiConsole.Ask<string>("Enter name of employee:");
            var startTime = AnsiConsole.Ask<DateTime>("Enter start time (MM-dd-YY HH:mm:ss):");
            var endTime = AnsiConsole.Ask<DateTime>("Enter end time (MM-dd-YY HH:mm:ss):");
            if (endTime < startTime)
            {
                AnsiConsole.MarkupLine($"End time: {endTime} cannot be less then start time: {startTime}");
                return false;
            }
            var shift = new Shift
            {
                Employee = employee,
                StartTime = startTime,
                EndTime = endTime
            };
            await ShiftsAPI.CreateShift(shift);
            return true;
        }

        public static async Task<bool> UpdateShift()
        {
            var id = AnsiConsole.Ask<int>("Enter id of updated shift:");
            var shift = await ShiftsAPI.GetShiftById(id);
            if (shift == null)
            {
                AnsiConsole.MarkupLine($"No shift with such id: {id}");
                return false;
            }

            var employee = AnsiConsole.Ask<string>("Enter name of employee:");
            var startTime = AnsiConsole.Ask<DateTime>("Enter start time (MM-dd-YY HH:mm:ss):");
            var endTime = AnsiConsole.Ask<DateTime>("Enter end time (MM-dd-YY HH:mm:ss):");
            if (endTime < startTime)
            {
                AnsiConsole.MarkupLine($"End time: {endTime} cannot be less then start time: {startTime}");
                return false;
            }
            var updatedShift = new Shift
            {
                Employee = employee,
                StartTime = startTime,
                EndTime = endTime
            };
            await ShiftsAPI.Update(id, updatedShift);
            return true;
        }

        public static async Task<bool> DeleteShift()
        {
            var id = AnsiConsole.Ask<int>("Enter id of deleted shift:");
            var shift = await ShiftsAPI.GetShiftById(id);
            if (shift == null)
            {
                AnsiConsole.MarkupLine($"No shift with such id: {id}");
                return false;
            }
            await ShiftsAPI.DeleteShift(id);
            return true;
        }
    }
}