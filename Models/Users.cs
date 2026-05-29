using System.ComponentModel.DataAnnotations;

namespace KiwiTaskAPI.Models
{
    public class Users
    {
        [Key]
        public Guid id { get; set; }
        public string? username { get; set; }

        public string? firstname { get; set; }

        public string? lastname { get; set; }

        public string? email { get; set; }

        public string? phone_number { get; set; }

        public string? avatar_url { get; set; }

        public string? bio { get; set; }

        public string social_links { get; set; } = null!;

        public string? introduction { get; set; }

        public string educations { get; set; } = null!;

        public string experiences { get; set; } = null!;

        public string skills { get; set; } = null!;

        public bool? is_recommand_required { get; set; }

        public string? wished_job_categories { get; set; }

        public string? wished_job_locations { get; set; }

        public string? wished_job_types { get; set; }

        public string roles { get; set; } = null!;

        public bool? is_active { get; set; }

        public string? refresh_token { get; set; }

        public DateTime? refresh_token_expiry { get; set; }


        public DateTime? created_at { get; set; }

        public DateTime? updated_at { get; set; }
        public string? reset_token { get; set; }
        public DateTime? reset_token_expiry { get; set; }

        public UserPassword? user_password { get; set; }

        public ICollection<Tasks> tasks { get; set; }

        public ICollection<PreferredCategories> preferred_categories { get; set; }


    }
}
