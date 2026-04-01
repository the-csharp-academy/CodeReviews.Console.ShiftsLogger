namespace ShiftsLoggerAPI.Models
{
    public class Shift
    {

        public int id { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public string JobTitle { get; set; } = string.Empty;

        public decimal HoursWorked
        {
            get
            {
                return (decimal)(end - start).TotalHours;
            }
        }

    }
}
