using System.ComponentModel.DataAnnotations;


namespace Database.Models.DTOs.Post
{
    public class AddNewPostDTO
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Text is required")]
        public string Text { get; set; }
        public string? Name { get; set; } 

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
