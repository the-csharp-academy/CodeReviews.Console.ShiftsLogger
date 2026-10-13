using ShiftsLoggerApi.Models;

namespace ShiftsLoggerApi
{
    public class ShiftResult
    {
        public ShiftsLog Shift { get; set; }

        public ShiftError ShiftError { get; set; }
    }
}