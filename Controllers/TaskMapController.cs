using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KiwiTaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskMapController : ControllerBase
    {
        private ITaskMapService _taskMapService;
        
        public TaskMapController(ITaskMapService taskMapService)
        {
            _taskMapService = taskMapService;
        }

        [HttpGet("map-pins")]
        public async Task<IActionResult> GetMapPins([FromQuery] GeoBounds? bbox, [FromQuery] int? limit)
        {
            var request = new MapPinsRequest(bbox, limit);

            return Ok();

        }
    }
}
