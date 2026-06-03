using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiwiTaskAPI.Models
{
    public class TaskOffers
    {
        [Key]
        public int id { get; set; }
        [Required]
        public Guid task_id { get; set; }
        [Required]
        public Guid user_id { get; set; }
        [Column(TypeName = "text")]
        public string? message { get; set; }
        [Column(TypeName = "decimal(10,2)")]
        public decimal price { get; set; }
        public string? attachments { get; set; }

        [Required]
        public DateTime created_at { get; set; }
        [Required]
        public DateTime expired_at { get; set; }

        public Users user { get; set; }

    }
}
