using KiwiTaskAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KiwiTaskAPI.Controllers
{
    [ApiController]
    [Route("api/tasks/{taskid}/offers")]
    public class OfferController : ControllerBase
    {
        public OfferController()
        {

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Guid taskid, [FromBody] OfferCreateDto dto)
        {
            Console.WriteLine($"taskid={taskid}");
            Console.WriteLine($"dto.price={dto.price}");
            Console.WriteLine($"dto.message={dto.message}");
            Console.WriteLine($"dto.attachments={dto.attachments}");

            return Ok();
        }
    }
}
