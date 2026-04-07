using System.Globalization;

namespace ShiftsLoggerUI.Validation
{
    internal class ShiftInputValidation
    {
        internal static bool ValidateEndTimeIsAfterStartTimeInput(string start, string end)
        {
            DateTime endTime = DateTime.ParseExact(end, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            DateTime startTime = DateTime.ParseExact(start, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            if (endTime < startTime)
            {
                return false;
            }

            return true;
        }
    }
}
