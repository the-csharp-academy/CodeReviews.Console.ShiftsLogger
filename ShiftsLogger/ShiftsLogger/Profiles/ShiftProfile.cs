using AutoMapper;
using ShiftsLogger.DTOs;
using ShiftsLogger.Models;

namespace ShiftsLogger.Profiles;

public class ShiftProfile : Profile
{
    public ShiftProfile()
    {
        CreateMap<Shift, ShiftCreateDTO>();
        CreateMap<ShiftCreateDTO, Shift>();
        CreateMap<Shift, ShiftReadDTO>();
    }
}
