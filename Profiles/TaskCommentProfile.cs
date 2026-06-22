using AutoMapper;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Profiles
{
    public class TaskCommentProfile: Profile
    {
        public TaskCommentProfile()
        {
            CreateMap<TaskCommentsDto, TaskComments>();
        }
    }
}
