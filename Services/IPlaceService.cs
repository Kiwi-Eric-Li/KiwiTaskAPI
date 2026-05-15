using KiwiTaskAPI.Dtos;

namespace KiwiTaskAPI.Services
{
    public interface IPlaceService
    {
        Task<List<PredictionDto>> AutocompleteAsync(string input, string? session);
    }
}
