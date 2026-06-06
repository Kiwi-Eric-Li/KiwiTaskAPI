using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KiwiTaskAPI.Controllers
{
    [ApiController]
    [Route("api/tasks/{taskid}/offers")]
    public class OfferController : ControllerBase
    {
        private readonly IOfferService _offerService;
        public OfferController(IOfferService offerService)
        {
            _offerService = offerService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetList(Guid taskid)
        {
            var result = await _offerService.GetTaskOffersAsync(taskid);
            return Ok(new
            {
                code = 0,
                data = result
            });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Guid taskid, [FromBody] OfferCreateDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _offerService.CreateOfferAsync(taskid, Guid.Parse(userId), dto);
            if(result > 0)
            {
                return Ok(new
                {
                    code = 0,
                    message = "make an offer successfully"
                });
            }
            else
            {
                return Ok(new
                {
                    code = 1,
                    message = "make an offer failed"
                });
            }
            
        }
    }
}
