using System.ComponentModel.DataAnnotations;

namespace KiwiTaskAPI.Models
{
    public class TaskMatches
    {
        [Key]
        public int id { get; set; }
        [Required]
        public Guid task_id { get; set; }
        [Required]
        public Guid tasker_id { get; set; }
        [Required]
        public DateTime matched_at { get; set; }
        [Required]
        public int confirmed { get; set; }
        [Required]
        public DateTime confirm_expires { get; set; }
    }
}
