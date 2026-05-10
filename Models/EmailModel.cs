namespace KiwiTaskAPI.Models
{
    public class EmailModel
    {
        public string? Name { get; set; }
        public string? CompanyName { get; set; }
        public string? FrontUrl { get; set; }
        public string? HeaderImage { get; set; }
        public string? FooterImage { get; set; }
        public string? Message { get; set; }
        public string? ResetPwdEmailToken { get; set; }
    }
}
