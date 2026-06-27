using System.ComponentModel.DataAnnotations;

namespace KiwiTaskAPI.Models
{
    public class TaskCompletionCodes
    {
        [Key]
        public int id { get; set; }
        [Required]
        public Guid task_id { get; set; }
        [Required]
        public string code { get; set; }
        [Required]
        public DateTime issued_at { get; set; }
        [Required]
        public DateTime expires_at { get; set; }
        [Required]
        public int used { get; set; }
        public DateTime? used_at { get; set; }
    }
}
