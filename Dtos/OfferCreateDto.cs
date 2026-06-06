namespace KiwiTaskAPI.Dtos
{
    public class OfferCreateDto
    {
        public decimal price { get; set; }
        public string? message { get; set; }
        public IEnumerable<string> attachments { get; set; }
    }
}
