using System.ComponentModel.DataAnnotations;

namespace KiwiTaskAPI.Models
{
    public class TaskCates
    {
        [Required]
        public string id { get; set; }
        [Required]
        public string title { get; set; }
        [Required]
        public Guid task_id { get; set; }
    }
}
