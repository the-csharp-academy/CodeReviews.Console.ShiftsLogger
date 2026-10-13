using System.ComponentModel.DataAnnotations;

namespace ShiftsLoggerApi.DTOs
{
    public class ShiftDto
    {
        [Required(ErrorMessage = "Worker id is required")]
        [Range(1, int.MaxValue)]
        public int WorkerId { get; set; }

        [Range(typeof(DateTime), "0001-01-01", "9999-12-31")]
        public DateTime StartDate { get; set; }

        [Range(typeof(DateTime), "0001-01-01", "9999-12-31")]
        public DateTime EndDate { get; set; }
    }
}