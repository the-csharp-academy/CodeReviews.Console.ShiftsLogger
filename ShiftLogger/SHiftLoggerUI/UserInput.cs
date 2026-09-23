using System.Globalization;

namespace ShiftLoggerUI;

public class UserInput
{
    public static string GetName(string message)
    {
        Console.WriteLine(message);

        bool validName = false;
        string? input = "";
        while (validName == false)
        {
            Console.WriteLine("Please insert name of the worker: ");
            input = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Invalid name.");
                validName = false;
            }
            else
            {
                input = input.Trim();
                foreach (char c in input)
                {
                    if (!Char.IsLetter(c))
                    {
                        Console.WriteLine("Invalid name.");
                        validName = false;
                        break;
                    }
                    validName = true;
                }
            }
        }
        return input!;
    }

    public static int GetId(string message)
    {
        Console.WriteLine(message);

        bool validId = false;
        string? input = "";
        int number = -1;

        input = Console.ReadLine();

        while (!validId)
        {
            validId = int.TryParse(input, out number);
            if (!validId)
            {
                Console.WriteLine("Invalid input. Enter the ID again:");
                input = Console.ReadLine();

            }
        }
        return number;
    }

    public static DateTime GetDateTime(string message)
    {
        Console.WriteLine(message);

        bool validDT = false;
        string? input = "";
        DateTime dt = DateTime.MinValue;

        input = Console.ReadLine();

        while (!validDT)
    {
        validDT = DateTime.TryParseExact(input, "dd-MM-yyyy - HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out dt);
        if (!validDT)
        {
            Console.WriteLine("Invalid input. Enter the Date and time again:");
            input = Console.ReadLine();
        }
    }
        return dt;
    }
}