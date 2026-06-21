namespace KiwiTaskAPI.Dtos
{
    public class TaskCommentsDto
    {
        public int id { get; set; }
        public Guid task_id { get; set; }
        public int? comment_id { get; set; }
        public Guid commenter_user_id { get; set; }
        public string content { get; set; }
        public string? attachments { get; set; }
        public DateTime created_at { get; set; }
        
        public UsersDto user { get; set; }
        public TasksDto task { get; set; }
    }
}
