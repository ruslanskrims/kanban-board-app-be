using AutoMapper;
using KanbanBoardAPI.Models;

namespace KanbanBoardAPI.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<KanbanTask, TaskDto>()
            .ForMember(dest => dest.StatusName,
                opt => opt.MapFrom(src => src.Status.ToString()));
    }
}