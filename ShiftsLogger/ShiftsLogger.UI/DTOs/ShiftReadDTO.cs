namespace ShiftsLogger.UI.DTOs
{
    public class ShiftReadDTO
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public string WorkerName { get; set; } = string.Empty;
        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
