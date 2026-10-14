namespace Shifts.Logger
{
    public class Shift
    {
        public int Id { get; set; }
        public string Employee { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public TimeSpan Duration => EndTime - StartTime;

        public override string ToString()
        {
            return Employee + "\t" + StartTime.ToString() + "\t" + EndTime.ToString() + "\t" + Duration.ToString();
        }
    }
}
