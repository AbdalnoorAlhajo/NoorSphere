using Database.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace Database.Models.DTOs.PostAndRelatedEntities.Post
{
    public class GetPostDTO
    {
        public string Text { get; set; }

        public string Name { get; set; }
        public List<Like> likes { get; set; }
        public List<Domain.Comment> comments { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
