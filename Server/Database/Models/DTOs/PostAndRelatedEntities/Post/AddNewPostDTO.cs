using System.ComponentModel.DataAnnotations;


namespace Database.Models.DTOs.Post
{
    public class AddNewPostDTO
    {

        [Required(ErrorMessage = "Text is required")]
        public string Text { get; set; }
        public string? Name { get; set; }

        public int? PostId { get; set; }

        public string? ImageURL { get; set; }
        
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
