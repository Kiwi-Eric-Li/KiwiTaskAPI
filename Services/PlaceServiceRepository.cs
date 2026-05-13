
using KiwiTaskAPI.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

namespace KiwiTaskAPI.Services
{
    public class PlaceServiceRepository : IPlacesService
    {
        private readonly HttpClient _http;
        private readonly string _key;
        private readonly IConfiguration _configuration;

        public PlaceServiceRepository(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
            _key = _configuration["GoogleApi:Key"];
        }

        public async Task<List<PredictionDto>> AutocompleteAsync(string input, string? session)
        {
            var query = new Dictionary<string, string?>(StringComparer.Ordinal)
            {
                ["input"] = input,
                ["components"] = "country:nz",
                ["types"] = "geocode",
                ["language"] = "en",
                ["key"] = _key
            };

            if (!string.IsNullOrEmpty(session))
            {
                query["sessiontoken"] = session;
            }
            var url = QueryHelpers.AddQueryString("place/autocomplete/json", query);
            var stream = await _http.GetStreamAsync(url, default);
            var json = await JsonDocument.ParseAsync(stream, default);

            var status = json.RootElement.GetProperty("status").GetString();
            if(status is not ("OK" or "ZERO_RESULTS"))
            {
                throw new HttpRequestException($"Google Places autocomplete error: {status}");
            }
            var list = new List<PredictionDto>();
            if(json.RootElement.TryGetProperty("predictions", out var preds))
            {
                foreach(var p in preds.EnumerateArray())
                {
                    var desc = p.GetProperty("description").GetString() ?? "";
                    var pid = p.GetProperty("place_id").GetString() ?? "";
                    if (!string.IsNullOrWhiteSpace(pid))
                    {
                        var predictionDto = new PredictionDto()
                        {
                            description = desc,
                            place_id = pid
                        };
                        list.Add(predictionDto);
                    }
                }
            }
            return list;
        }
    }
}
