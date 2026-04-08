using ShiftsLoggerUI.Models;
using ShiftsLoggerUI.Services;
using Spectre.Console;
using System.Globalization;

namespace ShiftsLoggerUI.Controllers
{
    internal class ShiftController
    {

        internal async Task LogShift()
        {
            Console.Clear();

            string jobTitle = AnsiConsole.Ask<string>("Enter your job title.");
            string startTime;
            string endTime;

            AnsiConsole.MarkupLine("Please enter the start time of your shift in the format: [underline][bold]yyyy-MM-dd HH:mm[/][/]");
            startTime = Console.ReadLine()!;


            while (!DateTime.TryParseExact(startTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                Console.WriteLine("Invalid input. Please enter a valid start time in the format: yyyy-MM-dd HH:mm");
                startTime = Console.ReadLine()!;
            }


            AnsiConsole.MarkupLine("Please enter the end time of your shift in the format: [underline][bold]yyyy-MM-dd HH:mm[/][/]");
            endTime = Console.ReadLine()!;

            while (!DateTime.TryParseExact(endTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                Console.WriteLine("Invalid input. Please enter a valid end time in the format: yyyy-MM-dd HH:mm");
                startTime = Console.ReadLine()!;
            }

            while (!Validation.ShiftInputValidation.ValidateEndTimeIsAfterStartTimeInput(startTime, endTime))
            {
                Console.WriteLine("End time must be after start time. Please enter a valid end time in the format: yyyy-MM-dd HH:mm");
                endTime = Console.ReadLine()!;
            }

            Shift shift = new Shift
            {
                JobTitle = jobTitle,
                start = DateTime.ParseExact(startTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                end = DateTime.ParseExact(endTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture)
            };
            ShiftsService shiftsService = new ShiftsService();

            await shiftsService.LogShift(shift);
            AnsiConsole.Status()
                         .Start("Adding shift to database", ctx =>
                         {
                             ctx.Spinner(Spinner.Known.Aesthetic);
                         });
        }

        internal async Task UpdateShift()
        {
            Console.WriteLine("What shift do you want to update?\n");

            var shiftsService = new ShiftsService();
            await shiftsService.ShowAllShifts(false);
            

            int shiftId = AnsiConsole.Ask<int>("Enter the [underline][bold]Shift ID[/][/] of the shift you want to update.");

            Console.WriteLine($"You want to update shift with ID: {shiftId}");
            var existingShift = await ShiftsService.GetShiftById(shiftId);

            if (existingShift == null)
            {
                Console.WriteLine("Shift not found. Please try again.");
                await UpdateShift();
                return;
            }

            var choice = AnsiConsole.Prompt(
               new SelectionPrompt<string>()
               .Title("What metric do you want to update")
               .AddChoices(new[]
               {
                    "Title",
                    "Start time",
                    "End time"
               }));

            switch (choice)
            {
                case "Title":
                {
                    
                    
                        existingShift.JobTitle = AnsiConsole.Ask<string>("Enter the new job title.");
                        await shiftsService.UpdateShift(shiftId, existingShift);
                    
                    break;
                }

                case "Start time":
                {
                    
                        string newStartTime = AnsiConsole.Ask<string>("Enter the new start time in the format: [underline][bold]yyyy-MM-dd HH:mm[/][/]");
                        while (!DateTime.TryParseExact(newStartTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                        {
                            Console.WriteLine("Invalid input. Please enter a valid start time in the format: yyyy-MM-dd HH:mm");
                            newStartTime = Console.ReadLine()!;
                        }
                        existingShift.start = DateTime.ParseExact(newStartTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                        await shiftsService.UpdateShift(shiftId, existingShift);
                    
                    break;
                }

                case "End time":
                {
                    
                        string newEndTime = AnsiConsole.Ask<string>("Enter the new end time in the format: [underline][bold]yyyy-MM-dd HH:mm[/][/]");
                        while (!DateTime.TryParseExact(newEndTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                        {
                            Console.WriteLine("Invalid input. Please enter a valid end time in the format: yyyy-MM-dd HH:mm");
                            newEndTime = Console.ReadLine()!;
                        }
                        if(existingShift.start > DateTime.ParseExact(newEndTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture))
                        {
                            Console.WriteLine("End time cannot be before start time. Update cancelled.");
                              Thread.Sleep(1000);
                            return;
                        }
                        existingShift.end = DateTime.ParseExact(newEndTime, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                        await shiftsService.UpdateShift(shiftId, existingShift);
                    
                    break;
                }
            }

        }

        internal async Task DeleteShift()
        {
            Console.WriteLine("What shift do you want to delete?\n");
            var shiftsService = new ShiftsService();
            await shiftsService.ShowAllShifts(false);

            int shiftId = AnsiConsole.Ask<int>("Enter the [underline][bold]Shift ID[/][/] of the shift you want to delete.");

            Console.WriteLine($"You want to delete shift with ID: {shiftId}");
            var confirmation = AnsiConsole.Confirm("Are you sure you want to delete this shift?");
            if (confirmation)
            {
                await shiftsService.DeleteShift(shiftId);
                Console.WriteLine("Shift deleted successfully.");
            }
            else
            {
                Console.WriteLine("Shift deletion cancelled.");
            }
        }
    }
}
