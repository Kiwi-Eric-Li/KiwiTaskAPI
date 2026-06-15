using AutoMapper;
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
        private readonly IMapper _mapper;
        public OfferController(IOfferService offerService, IMapper mapper)
        {
            _offerService = offerService;
            _mapper = mapper;
        }

        [HttpPost("cancel/{offerid}")]
        [Authorize]
        public async Task<IActionResult> CancelOffer(Guid taskid, int offerid)
        {
            var result = await _offerService.CancelOfferAsync(taskid, offerid);
            if (result > 0)
            {
                return Ok(new
                {
                    code = 0,
                    data = result
                });
            }
            else
            {
                return Ok(new
                {
                    code = 1,
                    data = 0
                });
            }
        }


        [HttpPost("accept/{taskerid}/{offerid}")]
        [Authorize]
        public async Task<IActionResult> AcceptOffer(Guid taskid, Guid taskerid, int offerid)
        {
            var result = await _offerService.AcceptOfferAsync(taskid, taskerid, offerid);
            if(result > 0)
            {
                return Ok(new
                {
                    code = 0,
                    data = result
                });
            }
            else
            {
                return Ok(new
                {
                    code = 1,
                    data = 0
                });
            }
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

        [HttpGet("refetch")]
        [Authorize]
        public async Task<IActionResult> GetOfferListByTaskId(Guid taskid)
        {
            var taskOffers = await _offerService.GetTaskOffersByTaskIdAsync(taskid);
            if(taskOffers is not null)
            {
                return Ok(new
                {
                    code = 0,
                    data = _mapper.Map<List<TaskOffersDto>>(taskOffers)
                });
            }
            else
            {
                return Ok(new
                {
                    code = 0,
                    data = Array.Empty<TaskOffersDto>()
                });
            }
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
