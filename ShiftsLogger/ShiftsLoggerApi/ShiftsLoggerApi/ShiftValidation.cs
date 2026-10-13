namespace ShiftsLoggerApi
{
    public class ShiftValidation
    {
        public bool IsDateCorrect(DateTime startDate, DateTime endDate)
        {
            if (endDate > startDate)
            {
                return true;
            }
            return false;
        }

        public bool IsIdCorrect(int id)
        {
            if (id >= 1)
            {
                return true;
            }
            return false;
        }
    }
}