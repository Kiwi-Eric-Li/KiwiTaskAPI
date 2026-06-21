using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiwiTaskAPI.Models
{
    public class TaskComments
    {
        [Key]
        public int id { get; set; }
        public Guid task_id { get; set; }
        public int? comment_id { get; set; }
        public Guid commenter_user_id { get; set; }
        [Required]
        public string content { get; set; }
        public string? attachments { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        [ForeignKey("commenter_user_id")]
        public virtual Users user { get; set; }

        [ForeignKey("task_id")]
        public virtual Tasks task { get; set; }
    }
}
