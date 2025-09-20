using ShiftLoggerAPI.Contracts;
using ShiftLoggerAPI.Entities;

namespace ShiftLoggerAPI.Mapping;

public static class EmployeeMapping
{
    public static EmployeeResponse ToResponse(this Employee e)
    {
        return new EmployeeResponse(e.Id, e.Name, e.Title);
    }
}
