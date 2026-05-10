using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KiwiTaskAPI.Models
{
    public class Attachment
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        public Guid uploader_id { get; set; }
        [Required]
        public string url { get; set; }
        [Required]
        public Guid ctx_id { get; set; }    // 该附件从属于哪个任务或评论
        [Required]
        public DateTime created_at { get; set; }
    }
}
