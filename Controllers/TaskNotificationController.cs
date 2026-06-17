using KiwiTaskAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;

namespace KiwiTaskAPI.Controllers
{
    [ApiController]
    [Route("api/task-notifications"), Authorize]
    public class TaskNotificationController : ControllerBase
    {
        private readonly ITaskNotificationService _notificationService;
        
        public TaskNotificationController(ITaskNotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("unread-count")]
        public async Task<IActionResult> GetUnReadNotificationCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var count = await _notificationService.GetUnReadNotificationCountAsync(Guid.Parse(userId));
            return Ok(new
            {
                code = 0,
                data = count
            });
        }



    }
}
