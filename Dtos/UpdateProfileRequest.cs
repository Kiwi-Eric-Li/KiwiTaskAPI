namespace KiwiTaskAPI.Dtos
{
    public class UpdateProfileRequest
    {
        public Guid id { get; set; }
        public string bio { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public IFormFile avatar { get; set; }
    }
}
