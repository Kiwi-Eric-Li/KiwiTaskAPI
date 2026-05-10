

using KiwiTaskAPI.Models;

namespace KiwiTaskAPI.Dtos
{
    public class TasksDto
    {
        // DTO 是面向UI的
        public Guid id { get; set; }
        
        public Guid poster_id { get; set; }
        
        public string title { get; set; }
       
        public string description { get; set; }
       
        public string type { get; set; }
       
        public string pricing_type { get; set; }
        
        public DateTime expires_at { get; set; }
        public decimal? estimated_hours { get; set; }
        public string? budget { get; set; }
        public decimal? budget_amount { get; set; }
        
        public string location { get; set; }
        public string? suburb { get; set; }
        public string? city { get; set; }
        public string? postcode { get; set; }
        public decimal? latitude { get; set; }
        public decimal? longitude { get; set; }
       
        public DateTime created_at { get; set; }
    }
}
