using System.ComponentModel.DataAnnotations;

namespace KiwiTaskAPI.Models
{
    public class TaskNotifications
    {
        [Key]
        public int id { get; set; }
        [Required]
        public Guid user_id { get; set; }
        [Required]
        public string type { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public string body { get; set; }
        public Guid? task_id { get; set; }
        public int? offer_id { get; set; }
        [Required]
        public int is_read { get; set; }
        [Required]
        public DateTime created_at { get; set; }
        [Required]
        public DateTime? read_at { get; set; }

    }
}
