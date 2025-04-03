using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.Domain
{
    public class Post
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [Required]
        public string Text { get; set; }

        public List<Like> likes { get; set; }
        public List<Comment> comments { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

    }

    public class Like
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }
    }

    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        [ForeignKey("Post")]
        public int PostId { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        public string Name { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
