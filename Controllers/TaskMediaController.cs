using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KiwiTaskAPI.Controllers
{
    [ApiController]
    [Route("api/task-media")]
    public class TaskMediaController: ControllerBase
    {
        private readonly ITaskMediaService _taskMediaService;

        public TaskMediaController(ITaskMediaService taskMediaService)
        {
            _taskMediaService = taskMediaService;
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> Upload([FromForm] TaskMediaUploadDto dto)
        {
            foreach(var file in dto.Files)
            {
                Console.WriteLine(file.FileName);
            }
            return Ok();
        }

    }
}
