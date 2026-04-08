using Newtonsoft.Json;

namespace ShiftsLoggerUI.Models
{

    public class Shifts
    {
        [JsonProperty("Shifts")]
        public required List<Shift> ShiftList { get; set; }
    }


    public class Shift
    {
        [JsonProperty("id")]
        public int id { get; set; }
        [JsonProperty("start")]
        public DateTime start { get; set; }
        [JsonProperty("end")]
        public DateTime end { get; set; }


        [JsonProperty("jobTitle")]
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

