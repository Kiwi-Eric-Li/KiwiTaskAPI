using AutoMapper;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Profiles
{
    public class TasksProfile: Profile
    {
        public TasksProfile()
        {   // 会自动映射两个类中名字相同的字段
            CreateMap<Tasks, TasksDto>()
                .ForMember(
                    dest => dest.type,
                    opt => opt.MapFrom(src => src.type.ToString())
                )
                .ForMember(
                    dest => dest.pricing_type,
                    opt => opt.MapFrom(src => src.pricing_type.ToString())
                );   
        }
    }
}
