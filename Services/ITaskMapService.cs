using KiwiTaskAPI.Dtos;

namespace KiwiTaskAPI.Services
{
    public interface ITaskMapService
    {
        Task<IEnumerable<TaskMapPinDto>> GetOpenPinsAsync(MapPinsRequest request);
    }
}
