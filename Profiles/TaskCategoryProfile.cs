using AutoMapper;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Profiles
{
    public class TaskCategoryProfile: Profile
    {
        public TaskCategoryProfile()
        {
            CreateMap<TaskCategory, TaskCategoryDto>();
        }
        
    }
}
