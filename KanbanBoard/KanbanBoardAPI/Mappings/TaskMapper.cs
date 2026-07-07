using AutoMapper;
using KanbanBoardAPI.DTO;
using KanbanBoardAPI.Models;

namespace KanbanBoardAPI.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<KanbanTask, TaskDto>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title ?? string.Empty));
    }
}