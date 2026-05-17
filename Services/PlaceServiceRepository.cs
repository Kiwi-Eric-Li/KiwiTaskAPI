
using KiwiTaskAPI.Dtos;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Json;

namespace KiwiTaskAPI.Services
{
    public class PlaceServiceRepository : IPlaceService
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

        public async Task<PlaceResolvedDto> GetDetailsAsync(string place_id, string? session)
        {
            var query = new Dictionary<string, string?>(StringComparer.Ordinal)
            {
                ["place_id"] = place_id,
                ["fields"] = "address_component,geometry,name,formatted_address",
                ["language"] = "en",
                ["key"] = _key
            };
            if (!string.IsNullOrEmpty(session))
            {
                query["sessiontoken"] = session;
            }
            var url = QueryHelpers.AddQueryString("place/details/json", query);

            var stream = await _http.GetStreamAsync(url, default);
            var json = await JsonDocument.ParseAsync(stream, default);
            var status = json.RootElement.GetProperty("status").GetString();
            
            if(status is not "OK")
            {
                throw new HttpRequestException($"Google Places details error: {status}");
            }
            var result = json.RootElement.GetProperty("result");

            string? Pick(string type)
            {
                if (!result.TryGetProperty("address_components", out var comps)) return null;
                foreach(var c in comps.EnumerateArray())
                {
                    foreach(var t in c.GetProperty("types").EnumerateArray())
                    {
                        if (string.Equals(t.GetString(), type, StringComparison.OrdinalIgnoreCase))
                            return c.GetProperty("long_name").GetString();
                    }
                }
                return null;
            }


            var suburb = Pick("sublocality_level_1") ?? Pick("neighborhood") ?? Pick("locality") ??
                        Pick("administrative_area_level_3") ?? Pick("premise") ?? Pick("postal_town") ??
                        Pick("political") ?? (result.TryGetProperty("name", out var nm) ? nm.GetString() : null);

            var city = Pick("locality") ?? Pick("administrative_area_level_2");
            var postcode = Pick("postal_code");

            double? lat = null, lng = null;
            if(result.TryGetProperty("geometry", out var geo) && geo.TryGetProperty("location", out var loc))
            {
                if(loc.TryGetProperty("lat", out var latEl))
                {
                    lat = latEl.GetDouble();
                }
                if(loc.TryGetProperty("lng", out var lngEl))
                {
                    lng = lngEl.GetDouble();
                }
            }
            return new PlaceResolvedDto(
                PlaceName: result.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : null,
                FormattedAddress: result.TryGetProperty("formatted_address", out var faEl) ? faEl.GetString() : null,
                Suburb: suburb,
                City: city,
                Postcode: postcode,
                Latitude: lat,
                Longitude: lng
            );
        }
    }
}
