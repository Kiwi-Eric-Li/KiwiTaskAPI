namespace KiwiTaskAPI.Dtos
{
    public class UsersDto
    {
        public Guid id { get; set; }
        public string username { get; set; }
        public string avatar_url { get; set; }
        public string email { get; set; }
        public string phone_number { get; set; }
        public string bio { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }

    }
}
