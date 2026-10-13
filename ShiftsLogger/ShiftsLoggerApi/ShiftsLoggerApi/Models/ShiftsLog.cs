using System.ComponentModel.DataAnnotations.Schema;

namespace ShiftsLoggerApi.Models
{
    public class ShiftsLog
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [NotMapped]
        public TimeSpan Duration { get; set; }
    }
}