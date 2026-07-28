using ShiftsLogger.UI.DTOs;

namespace ShiftsLogger.UI
{
    public static class InputHelper
    {
        public static WorkerReadDTO? ChooseWorker(List<WorkerReadDTO> workers)
        {
            if (workers == null || workers.Count == 0)
            {
                return null;
            }

            string? inp;
            int workerId = -1;

            Console.WriteLine("\nChoose a worker by ID or go back to menu by typing 0:\t");

            while (true)
            {
                Console.Write("Your choice:\t");
                inp = Console.ReadLine()?.Trim() ?? "";

                if (inp == "0")
                {
                    return null;
                }

                if (Validator.IsValidString(inp) && Int32.TryParse(inp, out workerId) && workers.Any(x => x.Id == workerId))
                {
                    break;
                }

                Console.WriteLine("\nInvalid worker ID. Please choose an ID from the list above or 0 to exit.");
            }

            return workers.FirstOrDefault(x => x.Id == workerId);
        }
    }
}
