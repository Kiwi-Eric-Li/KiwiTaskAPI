using KiwiTaskAPI.Dtos;

namespace KiwiTaskAPI.Services
{
    public interface IPlacesService
    {
        Task<List<PredictionDto>> AutocompleteAsync(string input, string? session);
    }
}
