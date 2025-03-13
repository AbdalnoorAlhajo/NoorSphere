using System.ComponentModel.DataAnnotations;

namespace Database.Models.DTOs.Post
{
    public class GetPostDTO
    {
        public int UserId { get; set; }

        public string Text { get; set; }

        public string Name { get; set; } 

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
