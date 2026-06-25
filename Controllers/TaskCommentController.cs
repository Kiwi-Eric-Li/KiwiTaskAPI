using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KiwiTaskAPI.Controllers
{
    [ApiController]
    [Route("api/task-comments")]
    public class TaskCommentController : ControllerBase
    {
        private readonly ITaskCommentService _taskCommentServiceRepository;

        public TaskCommentController(ITaskCommentService taskCommentServiceRepository)
        {
            _taskCommentServiceRepository = taskCommentServiceRepository;
        }

        [HttpPost]
        public async Task<IActionResult> createTaskComment(TaskCommentsDto dto)
        {
            var result = await _taskCommentServiceRepository.CreateTaskCommentAsync(dto);
            return Ok(new
            {
                code = 0,
                data = result
            });
        }

        [HttpPost("reply")]
        public async Task<IActionResult> replyTaskComment(TaskCommentsDto dto)
        {
            var result = await _taskCommentServiceRepository.ReplyTaskCommentAsync(dto);
            return Ok(new
            {
                code = 0,
                data = result
            });
        }


    }
}
