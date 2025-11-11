namespace ShiftLogger.Models.ShiftLoggerDto;

public class ShiftLoggerDto
{
    public class EmployeeListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ShiftInDto
    {
        public int EmployeeId { get; set; }
    }
    public class ShiftOutDto
    {
        public int EmployeeId { get; set; }
    }
    public class ShiftLogDto
    {
        public string Name { get; set; }
        public int ShiftStatus { get; set; }
        public decimal TotalWorkingHours { get; set; }
    }
}
