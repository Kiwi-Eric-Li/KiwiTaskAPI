using KiwiTaskAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace KiwiTaskAPI.Controllers
{
    [Route("api/[conroller]")]
    [ApiController]
    public class PlaceController : ControllerBase
    {
        private PlaceServiceRepository _placesServiceRepository; 
        public PlaceController(PlaceServiceRepository placesServiceRepository)
        {
            _placesServiceRepository = placesServiceRepository;
        }


    }
}
