using System.Globalization;

namespace ShiftLoggerUI;

public class UserInput
{
    public static string GetName(string message)
    {  
        bool validName = false;
        string? input = "";
        while (validName == false)
        {
            Console.WriteLine(message);
            input = Console.ReadLine();

            if (String.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Invalid name.");
                validName = false;
            }
            else
            {
                input = input.Trim();

                if (!Char.IsLetter(input[0]))
                {
                    Console.WriteLine("Invalid name, should start with a letter.");
                    validName = false;
                    continue;
                }
                foreach (char c in input)
                {
                    if (!Char.IsLetter(c) && !Char.IsWhiteSpace(c) && !Char.Equals(c, '-') && !Char.Equals(c, '\''))
                    {
                        Console.WriteLine("Invalid name, should only contain letters, spaces, hyphens or apostrophes.");
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
        Console.WriteLine("Type 0 to cancel the operation.");

        bool validId = false;
        string? input = "";
        int number = -1;

        input = Console.ReadLine();

        while (!validId)
        {
            validId = int.TryParse(input, out number);
            if (number < 0)
            {
                validId = false;
            }
            if (!validId)
            {
                Console.WriteLine("Invalid input. Enter the ID again:");
                input = Console.ReadLine();

            }
        }
        return number;
    }

    public static (DateTime, DateTime) GetDateTimes(string message)
    {
        bool startBeforeEnd = false;
        DateTime dtStart = DateTime.MinValue;
        DateTime dtEnd = DateTime.MinValue;

        while (!startBeforeEnd)
        {
            dtStart = GetDateTime(message);
            dtEnd = GetDateTime("Please enter the date and time of the shift end - format [dd-MM-yyyy] - [HH:mm:ss]: ");


            if (dtStart >= dtEnd)
            {
                Console.WriteLine("Invalid input. The start time must be before the end time.");
                startBeforeEnd = false;
            }
            else
            {
                startBeforeEnd = true;
            }
        }

        return (dtStart, dtEnd);
    }

    public static DateTime GetDateTime(string message)
    {
        Console.WriteLine(message);

        bool validDt = false;
        string? input = "";
        DateTime dt = DateTime.MinValue;

        input = Console.ReadLine();

        while (!validDt)
        {
            validDt = DateTime.TryParseExact(input, "dd-MM-yyyy - HH:mm:ss", new CultureInfo("en-US"), DateTimeStyles.None, out dt);
            if (!validDt)
            {
                Console.WriteLine("Invalid input. Enter the Date and time again:");
                input = Console.ReadLine();
            }
        }
        return dt;
    }
}