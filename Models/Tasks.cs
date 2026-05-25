using KiwiTaskAPI.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace KiwiTaskAPI.Models
{
    // Model 是面向业务的
    public class Tasks
    {
        [Key]
        public Guid id { get; set; }
        [Required]
        public Guid poster_id { get; set; }
        [Required]
        [MaxLength(100)]
        public string title { get; set; }
        [Required]
        public string description { get; set; }
        [Required]
        public TaskType task_type { get; set; }    // remote, offline
        [Required]
        public PricingType pricing_type { get; set; }       // fixed, hourly
        public string? budget { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? estimated_hours { get; set; }
        [Required]
        public DateTime expires_at { get; set; }

        public string? location { get; set; }
        public string? suburb { get; set; }
        public string? city { get; set; }
        public string? postcode { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
        [Required]
        public DateTime schedule_time { get; set; }

        public virtual ICollection<TaskCates> categories { get; set; } = new List<TaskCates>();
        public virtual ICollection<TaskAttachment> task_attachments { get; set; } = new List<TaskAttachment>();  // 一个任务，会有多个附件
        [Required]
        public DateTime created_at { get; set; }
        [Required]
        public DateTime updated_at { get; set; }

    }
}
