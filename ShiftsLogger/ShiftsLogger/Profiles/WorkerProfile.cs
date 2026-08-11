using AutoMapper;
using ShiftsLogger.DTOs;
using ShiftsLogger.Models;

namespace ShiftsLogger.Profiles;

public class WorkerProfile : Profile
{
    public WorkerProfile()
    {
        CreateMap<Worker, WorkerDTO>();
        CreateMap<WorkerDTO, Worker>();
        CreateMap<Worker, WorkerReadDTO>();
    }
}
