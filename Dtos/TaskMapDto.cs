using Org.BouncyCastle.Asn1.Mozilla;

namespace KiwiTaskAPI.Dtos
{
    public record GeoBounds(double West, double South, double East, double North);
    public record TaskMapPinDto
    {
        public int Id { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string Title { get; init; } = "";
        public long Poster_id { get; init; }
        public string poster_name { get; init; } = "";
        public string poster_avatar_url { get; init; } = "";
        public decimal? Budget { get; init; }
        public string status { get; init; } = "";
        public DateTime? PostedAtUtc { get; init; }
        public DateTime? DueAtUtc { get; init; }
        public string? LocationTxt { get; init; }
    }
    public record MapPinsRequest(GeoBounds? Bounds, int? Limit);
    
}
