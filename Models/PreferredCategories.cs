using System.ComponentModel.DataAnnotations;

namespace KiwiTaskAPI.Models
{
    public class PreferredCategories
    {
        [Key]
        public int id { get; set; }
        [Required]
        public Guid user_id { get; set; }
        [Required]
        public int category_id { get; set; }

        public Users user { get; set; }
    }
}
