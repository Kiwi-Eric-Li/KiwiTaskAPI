using Aliyun.OSS;
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
        private readonly IOssService _oss;

        public TaskMediaController(ITaskMediaService taskMediaService, IOssService oss)
        {
            _taskMediaService = taskMediaService;
            _oss = oss;
        }

        [HttpPost("upload")]
        [Authorize]
        public async Task<IActionResult> Upload([FromForm] TaskMediaUploadDto dto)
        {
            List<string> urls = new List<string>();
            foreach(var file in dto.Files)
            {
                using var stream = file.OpenReadStream();
                var newUrl = await _oss.UploadAvatarAsync(stream, file.FileName);
                urls.Add(newUrl);
            }
            return Ok(new
            {
                code = 0, 
                data = urls
            });
        }

    }
}
