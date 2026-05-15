using KiwiTaskAPI.Dtos;
using KiwiTaskAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KiwiTaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private IPlaceService _placesServiceRepository; 
        public PlaceController(IPlaceService placesServiceRepository)
        {
            _placesServiceRepository = placesServiceRepository;
        }

        [HttpGet("autocomplete")]
        public async Task<ActionResult<List<PredictionDto>>> AutoComplete([FromQuery] string q, [FromQuery] string? session)
        {
            if (string.IsNullOrWhiteSpace(q))
            {
                return Ok(new {code = 0, data = Array.Empty<PredictionDto>() });
            }
            var result = await _placesServiceRepository.AutocompleteAsync(q, session);
            return Ok(new
            {
                code = 0,
                data = result
            });
        }
    }
}
