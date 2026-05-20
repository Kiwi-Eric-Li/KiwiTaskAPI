using AutoMapper;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Profiles
{
    public class TaskAttachmentProfile: Profile
    {
        public TaskAttachmentProfile()
        {
            CreateMap<TaskAttachment, TaskAttachmentDto>();
        }
    }
}
