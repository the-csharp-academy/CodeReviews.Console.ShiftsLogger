namespace Shifts.WebAPIs.Models
{
    public class Shift
    {
        public int Id { get; set; }
        public string Employee {  get; set; } = string.Empty;
        public DateTime StartTime {  get; set; }
        public DateTime EndTime {  get; set; }
        public TimeSpan Duration => EndTime - StartTime;
    }
}
