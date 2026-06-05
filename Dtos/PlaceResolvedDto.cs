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

    public record TaskMediaConfirmDto
    (
        string Url,
        string ContextType,
        long ContextId
    );

    public record TaskMediaUploadDto(IFormFile[] Files);
}
