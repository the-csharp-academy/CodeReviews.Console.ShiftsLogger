using Spectre.Console;
using System.Globalization;

namespace ShiftsLoggerApiClient
{
    internal class Menu
    {
        private readonly ApiConnection apiConnection = new ApiConnection();
        private readonly CultureInfo culture = CultureInfo.InvariantCulture;

        private readonly string[] dateFormats = { "H:m d-M-yyyy", "H:m d:M:yyyy", "H-m d-M-yyyy", "H.m d.M.yyyy", "H,m d,M,yyyy", "H-m d:M:yyyy", "H/m d/M/yyyy", "H:m d/M/yyyy",
        "H-m d/M/yyyy", "H:m"};

        public async Task MainMenu()
        {
            bool isRunning = true;
            while (isRunning)
            {
                string menuChoice = MenuSelection();
                switch (menuChoice)
                {
                    case "View Shifts":
                        Console.Clear();
                        await ViewShifts();
                        break;

                    case "Create Shift":
                        Console.Clear();

                        await CreateShift();
                        break;

                    case "Update Shift":
                        Console.Clear();

                        await UpdateShift();
                        break;

                    case "Delete Shift":
                        Console.Clear();

                        await DeleteShift();
                        break;

                    case "Exit Program":
                        Console.Clear();

                        isRunning = false;
                        Environment.Exit(0);
                        break;
                }
            }
        }

        public string MenuSelection()
        {
            var menu = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("What you want to do?")
                .AddChoices("View Shifts", "Create Shift", "Update Shift", "Delete Shift", "Exit Program"));

            return menu;
        }

        public async Task ViewShifts()
        {
            var shifts = await apiConnection.GetShifts();
            if (shifts.Success == false && shifts.Status != null)
            {
                AnsiConsole.MarkupLine($"[red]API returned error:[/] {shifts.Status}");
                return;
            }
            if (shifts.Success == false && shifts.Status == null)
            {
                AnsiConsole.MarkupLine($"[red]API doesn't respond:[/]");
                return;
            }
            if (!shifts.Shifts.Any())
            {
                AnsiConsole.MarkupLine($"[red]Error:[/]Shifts list is empty");
                return;
            }

            var viewShifts = new Table()
                .RoundedBorder()
                .ShowRowSeparators()
                .AddColumns("ShiftId", "WorkerId", "Start date", "End date", "Duration");

            foreach (ShiftsLog shift in shifts.Shifts)
            {
                viewShifts.AddRow(
                    Convert.ToString(shift.Id),
                    Convert.ToString(shift.WorkerId),
                    shift.StartDate.ToString("H:mm d-M-yyyy"),
                    shift.EndDate.ToString("H:mm d-M-yyyy"),
                    ($"{shift.Duration.Days} days" +
                    $" {shift.Duration.Hours} hours" +
                    $" {shift.Duration.Minutes} minutes"));
            }

            AnsiConsole.Write(viewShifts);
        }

        public async Task CreateShift()
        {
            var newShift = new ShiftDto();
            int workerId = AnsiConsole.Ask<int>("Please type new [LightGoldenrod2_2]worker id[/]");
            while (workerId < 1)
            {
                workerId = AnsiConsole.Ask<int>("Please type new [LightGoldenrod2_2]worker id[/]");
            }
            bool keepValidating = true;
            DateTime endDate = default;
            DateTime startDate = default;
            bool startSuccess = false;

            while (keepValidating)
            {
                var startDateString = AnsiConsole.Ask<string>("Please type new [LightGoldenrod2_2]start date H:m d-M-yyyy[/] or \"t\" for now");
                if (startDateString == "t")
                {
                    DateTime now = DateTime.Now;
                    startDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0);
                    startSuccess = true;
                }
                else
                {
                    startSuccess = DateTime.TryParseExact(startDateString, dateFormats, culture, DateTimeStyles.None, out startDate);
                }
                while (!startSuccess)
                {
                    Console.Clear();
                    AnsiConsole.MarkupLine("Make sure to enter the date in this order - [LightGoldenrod2_2]Hours:Minutes Day-Month-Year[/]");
                    startDateString = AnsiConsole.Ask<string>("Please type new [LightGoldenrod2_2]start date H:m d-M-yyyy[/] or [LightGoldenrod2_2]\"t\"[/] for now");
                    startSuccess = DateTime.TryParseExact(startDateString, dateFormats, culture, DateTimeStyles.None, out startDate);
                    if (startDateString == "t")
                    {
                        DateTime now = DateTime.Now;
                        startDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0);
                        startSuccess = true;
                    }
                }

                var endDateString = AnsiConsole.Ask<string>("Please type new [LightGoldenrod2_2]end date Hour:Minute Day-Month-Year[/] or use only [LightGoldenrod2_2]Hour:Minute[/] if ends same day");
                bool endSuccess = DateTime.TryParseExact(endDateString, dateFormats, culture, DateTimeStyles.None, out endDate);
                while (!endSuccess)
                {
                    AnsiConsole.MarkupLine("Make sure to enter the date in this order - [LightGoldenrod2_2]Hours:Minutes Day-Month-Year[/]");
                    endDateString = AnsiConsole.Ask<string>("Please type new [LightGoldenrod2_2]end date Hour:Minute Day-Month-Year[/] or use only [LightGoldenrod2_2]Hour:Minute[/] if ends same day");
                    endSuccess = DateTime.TryParseExact(endDateString, dateFormats, culture, DateTimeStyles.None, out endDate);
                }
                if (endDate <= startDate)
                {
                    keepValidating = true;
                    AnsiConsole.MarkupLine("End date must be [LightGoldenrod2_2]later[/] than start date");
                }
                else
                {
                    keepValidating = false;
                }
            }

            newShift.WorkerId = workerId;
            newShift.StartDate = startDate;
            newShift.EndDate = endDate;
            var result = await apiConnection.CreateShift(newShift);
            if (result.Success == false && result.Status == null)
            {
                AnsiConsole.MarkupLine("[red]Error: API doesn't respond:[/]");
                return;
            }
            if (result.Success == false && result.Status != null)
            {
                AnsiConsole.MarkupLine($"[red]API returned error:[/]{result.Status}");
                return;
            }
            AnsiConsole.MarkupLine("Shift [green]successfully[/] created");
        }

        public async Task UpdateShift()
        {
            var shifts = await apiConnection.GetShifts();
            if (shifts.Success == false && shifts.Status != null)
            {
                AnsiConsole.MarkupLine($"[red]API returned error:[/] {shifts.Status}");
                return;
            }
            if (shifts.Success == false && shifts.Status == null)
            {
                AnsiConsole.MarkupLine($"[red]API doesn't respond:[/]");
                return;
            }
            if (!shifts.Shifts.Any())
            {
                AnsiConsole.MarkupLine($"[red]Error:[/]Shifts list is empty");
                return;
            }

            var newShift = new ShiftDto();
            var menu = new SelectionPrompt<ShiftsLog>()
                .Title("Choose shift to update");
            foreach (ShiftsLog shift in shifts.Shifts)
            {
                menu.AddChoice(shift);
            }

            menu.Converter = shift =>
            $"{shift.Id,-5} Id | " +
            $"{shift.WorkerId,-5} WorkerId | " +
            $"{shift.StartDate,-5:HH:mm dd-MM-yyyy}-Start | " +
            $"{shift.EndDate,-5:HH:mm dd-MM-yyyy}-End | " +
            $"{shift.Duration.Days,-5} Days | " +
            $"{shift.Duration.Hours,-5} Hours | " +
            $"{shift.Duration.Minutes,-5} Minutes |";
            var selectedShift = AnsiConsole.Prompt(menu);

            DateTime startDate = default;
            DateTime endDate = default;

            int newWorkerId = AnsiConsole.Ask<int>("Please type new [LightGoldenrod2_2]worker id[/]");
            while (newWorkerId < 1)
            {
                newWorkerId = AnsiConsole.Ask<int>("Please type new [LightGoldenrod2_2]worker id[/]");
            }
            bool keepValidating = true;
            bool newStartDate = false;
            while (keepValidating)
            {
                string newStartDateString = AnsiConsole.Ask<string>("Please type new [LightGoldenrod2_2]start date H:m d-M-yyyy[/] or \"t\" for now");
                if (newStartDateString == "t")
                {
                    DateTime now = DateTime.Now;
                    startDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0);
                    newStartDate = true;
                }
                else
                {
                    newStartDate = DateTime.TryParseExact(newStartDateString, dateFormats, culture, DateTimeStyles.None, out startDate);
                }
                while (!newStartDate)
                {
                    AnsiConsole.MarkupLine("Make sure to enter the date in this order - [LightGoldenrod2_2]Hours:Minutes Day-Month-Year[/]");
                    newStartDateString = AnsiConsole.Ask<string>("Please type new [LightGoldenrod2_2]start date H:m d-M-yyyy[/] or \"t\" for now");
                    newStartDate = DateTime.TryParseExact(newStartDateString, dateFormats, culture, DateTimeStyles.None, out startDate);
                    if (newStartDateString == "t")
                    {
                        DateTime now = DateTime.Now;
                        startDate = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0, 0);
                        newStartDate = true;
                    }
                }

                string newEndDatestring = AnsiConsole.Ask<string>("Please type new [LightGoldenrod2_2]end date H:m d-M-yyyy[/] or use only [LightGoldenrod2_2]Hour:Minute[/] if ends same day");
                bool newEndDate = DateTime.TryParseExact(newEndDatestring, dateFormats, culture, DateTimeStyles.None, out endDate);
                while (!newEndDate)
                {
                    AnsiConsole.MarkupLine("Make sure to enter the date in this order - [LightGoldenrod2_2]Hours:Minutes Day-Month-Year[/]");
                    newEndDatestring = AnsiConsole.Ask<string>("Please type new [LightGoldenrod2_2]end date Hour:Minute Day-Month-Year[/] or use only[LightGoldenrod2_2] Hour:Minute[/] if ends same day");
                    newEndDate = DateTime.TryParseExact(newEndDatestring, dateFormats, culture, DateTimeStyles.None, out endDate);
                }
                if (endDate <= startDate)
                {
                    keepValidating = true;
                    AnsiConsole.MarkupLine("End date must be [LightGoldenrod2_2]later[/] than start date");
                }
                else
                {
                    keepValidating = false;
                }
            }

            newShift.WorkerId = newWorkerId;
            newShift.StartDate = startDate;
            newShift.EndDate = endDate;
            var result = await apiConnection.UpdateShift(selectedShift.Id, newShift);
            if (result.Success == false && result.Status == null)
            {
                AnsiConsole.MarkupLine("[red]Error: API doesn't respond:[/]");
                return;
            }
            if (result.Success == false && result.Status != null)
            {
                AnsiConsole.MarkupLine($"[red]API returned error:[/]{result.Status}");
                return;
            }
            AnsiConsole.MarkupLine("Shift [green]successfully[/] updated");
        }

        public async Task DeleteShift()
        {
            var shifts = await apiConnection.GetShifts();

            if (shifts.Success == false && shifts.Status != null)
            {
                AnsiConsole.MarkupLine($"[red]API returned error:[/] {shifts.Status}");
                return;
            }
            if (shifts.Success == false && shifts.Status == null)
            {
                AnsiConsole.MarkupLine($"[red]API doesn't respond:[/]");
                return;
            }
            if (!shifts.Shifts.Any())
            {
                AnsiConsole.MarkupLine($"[red]Error:[/]Shifts list is empty");
                return;
            }
            var menu = new SelectionPrompt<ShiftsLog>()
                .Title("Choose shift to delete");
            foreach (ShiftsLog shift in shifts.Shifts)
            {
                menu.AddChoice(shift);
            }

            menu.Converter = shift =>
                $"{shift.Id,-5} Id | " +
                $"{shift.WorkerId,-5} WorkerId | " +
                $"{shift.StartDate,-5:HH:mm dd-MM-yyyy}-Start | " +
                $"{shift.EndDate,-5:HH:mm dd-MM-yyyy}-End | " +
                $"{shift.Duration.Days,-5} Days | ";
            var selectedShift = AnsiConsole.Prompt(menu);
            int id = selectedShift.Id;

            var result = await apiConnection.DeleteShift(id);
            if (result.Success == false && result.Status == null)
            {
                AnsiConsole.MarkupLine("[red]Error: API doesn't respond:[/]");
                return;
            }
            if (result.Success == false && result.Status != null)
            {
                AnsiConsole.MarkupLine($"[red]API returned error:[/]{result.Status}");
                return;
            }
            AnsiConsole.MarkupLine("Shift [green]successfully[/] deleted");
        }
    }
}