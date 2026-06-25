using AutoMapper;
using KiwiTaskAPI.Database;
using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Models;
using System.Text.Json;

namespace KiwiTaskAPI.Services
{
    public class TaskCommentServiceRepository : ITaskCommentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public TaskCommentServiceRepository(IMapper mapper, AppDbContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<int> ReplyTaskCommentAsync(TaskCommentsDto dto)
        {
            var taskComments = _mapper.Map<TaskComments>(dto);
            taskComments.created_at = DateTime.Now;
            taskComments.updated_at = DateTime.Now;
            await _context.task_comments.AddAsync(taskComments);
            return await _context.SaveChangesAsync();

        }


        public async Task<int> CreateTaskCommentAsync(TaskCommentsDto dto)
        {
            var taskComments = _mapper.Map<TaskComments>(dto);

            //if(taskComments.attachments.Count() > 0)
            //{
            //    var attachments = new List<object>();
            //    foreach(var at in dto.attachments)
            //    {
            //        attachments.Add(new
            //        {
            //            url = at,
            //            type = "image"
            //        });
            //    }
            //    taskComments.attachments = attachments;
            //}
            //else
            //{
            //    taskComments.attachments = [];
            //}
            taskComments.created_at = DateTime.Now;
            taskComments.updated_at = DateTime.Now;
            await _context.task_comments.AddAsync(taskComments);
            return await _context.SaveChangesAsync();
        }
    }
}
