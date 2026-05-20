using KiwiTaskAPI.Services;
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


    }
}
