using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Required]
        public string Text { get; set; }

        public string? Name { get; set; } // Future enhancement: Remove this property in favor of dynamically fetching the 'Name' related entity (User). 


        public DateTime Date { get; set; } = DateTime.UtcNow;

    }

    public class Like
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }

    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }

        [Required]
        public string Text { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
