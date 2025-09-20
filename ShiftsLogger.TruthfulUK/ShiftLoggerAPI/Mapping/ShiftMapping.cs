using ShiftLoggerAPI.Contracts;
using ShiftLoggerAPI.Entities;

namespace ShiftLoggerAPI.Mapping;

public static class ShiftMapping
{
    public static ShiftResponse ToResponse(this Shift s)
    {
        return new ShiftResponse(s.Id, s.Start, s.End, s.Duration);
    }
}
