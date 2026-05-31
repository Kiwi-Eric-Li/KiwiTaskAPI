using System.ComponentModel.DataAnnotations;

namespace KiwiTaskAPI.Models
{
    public class NotificationSettings
    {
        [Key]
        public int id { get; set; }
        [Required]
        public Guid user_id { get; set; }
        [Required]
        public int app_enabled { get; set; }
        [Required]
        public int email_enabled { get; set; }
        [Required]
        public int marketing_opt { get; set; }

        public Users user { get; set; }

    }
}
