namespace KiwiTaskAPI.Dtos
{
    public record PlaceResolvedDto
    (
        string? PlaceName,
        string? FormattedAddress,
        string? Suburb,
        string? City,
        string? Postcode,
        double? Latitude,
        double? Longitude
    );
}
