

using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Dtos
{
    public class TasksDto
    {
        public Guid id { get; set; }
        
        public Guid poster_id { get; set; }
        
        public string title { get; set; }
       
        public string description { get; set; }
       
        public TaskType task_type { get; set; }
       
        public string pricing_type { get; set; }
        public string? budget { get; set; }
        public decimal? estimated_hours { get; set; }
        public DateTime expires_at { get; set; }
        public string? location { get; set; }
        public string? suburb { get; set; }
        public string? city { get; set; }
        public string? postcode { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        public DateTime schedule_time { get; set; }
        public DateTime created_at { get; set; }
        public ICollection<TaskCatesDto> categories { get; set; }
        public ICollection<TaskAttachmentDto> task_attachments { get; set; }
        public string status { get; set; }
        public UsersDto poster { get; set; }
        public ICollection<TaskOffers> offers { get; set; } = new List<TaskOffers>();
    }
}
