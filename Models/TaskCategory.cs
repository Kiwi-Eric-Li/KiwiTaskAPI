using System.ComponentModel.DataAnnotations;

namespace KiwiTaskAPI.Models
{
    public class TaskCategory
    {
        [Key]
        public string id { get; set; }
        [Required]
        public string title { get; set; }
        public bool? is_active { get; set; }
    }
}
