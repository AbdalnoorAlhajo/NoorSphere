

using Database.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Database.Models.DTOs.ProfileAndRelatedEntities.Profile
{
    public class GetProfileDTO
    {
        public string UserId { get; set; }

        public string? Company { get; set; }
        public string? Website { get; set; }
        public string? Country { get; set; }
        public string? Location { get; set; }
        public string? AvatarUrl { get; set; }


        [Required]
        public string Status { get; set; }

        [Required]
        public string Skills { get; set; }

        public string? Bio { get; set; }
        public List<Database.Models.Domain.Experience> Experiences { get; set; }
        public List<Database.Models.Domain.Education> Educations { get; set; }
        public SocialLinks SocialLinks { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
