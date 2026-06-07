namespace KiwiTaskAPI.Dtos
{
    public class TaskOffersDto
    {
        public int id { get; set; }
        public Guid task_id { get; set; }
        public Guid user_id { get; set; }
        public decimal price { get; set; }
        public string? message { get; set; }
        public string? attachments { get; set; }
        public DateTime created_at { get; set; }
        public DateTime expired_at { get; set; }

        public UsersDto user { get; set; }

        public bool is_expired { get; set; }
        public bool is_matched { get; set; }

    }
}
