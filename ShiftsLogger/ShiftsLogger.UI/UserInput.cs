using ShiftsLogger.UI.Services;
using System;

namespace ShiftsLogger.UI
{
    public class UserInput
    {
        private readonly WorkerApiService _workerApiService;
        private readonly ShiftApiService _shiftApiService;

        private UserInput(WorkerApiService workerApiService, ShiftApiService shiftApiService)
        {
            _workerApiService = workerApiService;
            _shiftApiService = shiftApiService;
        }

        public static async Task RunAsync(WorkerApiService workerApiService, ShiftApiService shiftApiService)
        {
            var input = new UserInput(workerApiService, shiftApiService);
            await input.GetMainMenuInput();
        }

        private async Task GetMainMenuInput()
        {
            Console.Clear();
            List<string> choices = new();
            choices.Add("Exit");
            choices.Add("Manage workers.");
            choices.Add("Manage shifts.");
            TableVisualizer.ShowChoices(choices, "MAIN MENU");

            string? inp = Console.ReadLine();
            while (!Validator.IsValidChoice(inp, 2))
            {
                Console.Write("\nPlease enter a valid number!\nTry again:\n> ");
                inp = Console.ReadLine();
            }
            int choice = int.Parse(inp!);

            switch (choice)
            {
                case 0:
                    Console.WriteLine("\nGood bye!");
                    break;
                case 1:
                    await WorkerInput.RunAsync(_workerApiService, _shiftApiService);
                    Console.WriteLine("\nPress any key to go back to the menu.");
                    Console.ReadKey();
                    await GetMainMenuInput();
                    break;
                case 2:
                    await ShiftInput.RunAsync(_workerApiService, _shiftApiService);
                    Console.WriteLine("\nPress any key to go back to the menu.");
                    Console.ReadKey();
                    await GetMainMenuInput();
                    break;
                default:
                    break;
            }
        }
    }
}
