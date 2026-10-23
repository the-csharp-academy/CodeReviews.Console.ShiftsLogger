[ValidShiftTimeRange]
public class CreateShiftDTO : IShiftTimeRange
{
    public int EmployeeID { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}