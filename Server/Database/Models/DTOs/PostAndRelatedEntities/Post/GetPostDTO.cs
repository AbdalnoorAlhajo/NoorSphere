using Database.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace Database.Models.DTOs.PostAndRelatedEntities.Post
{
    public class GetPostDTO
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public string Name { get; set; }
        public int likes { get; set; }
        public string? ImageURL { get; set; }

        public string? AvatarURL { get; set; }

        public bool IsLiked { get; set; }

        public int comments { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
