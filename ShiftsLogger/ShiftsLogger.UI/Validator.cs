using System;
using System.Text.RegularExpressions;

namespace ShiftsLogger.UI
{
    public static class Validator
    {
        public static bool IsValidChoice(string? strInput, int num)
        {
            if (String.IsNullOrEmpty(strInput))
            {
                return false;
            }

            if (!int.TryParse(strInput, out int choice))
            {
                return false;
            }
            return 0 <= choice && choice <= num;
        }

        public static bool IsValidString(string? stringInput)
        {
            if (String.IsNullOrEmpty(stringInput))
            {
                return false;
            }

            foreach (char c in stringInput)
            {
                if (!(Char.IsLetter(c) || Char.IsDigit(c)) && c != '/' && c != ' ')
                {
                    return false;
                }
            }

            return true;
        }
    }
}
