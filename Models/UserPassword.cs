using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace KiwiTaskAPI.Models
{
    public class UserPassword
    {
        [Key]
        public int id { get; set; }
        
        [Required]
        public string password_hash { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
        [Required]
        public Guid user_id { get; set; }
        [JsonIgnore]
        public Users user { get; set; }
    }
}
