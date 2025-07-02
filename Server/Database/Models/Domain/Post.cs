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

        public string? ImageURL { get; set; }

        public List<Like> likes { get; set; }


        // If this post is a comment, PostId refers to the parent post's Id.
        // If PostId is null, then this is a top-level post (not a comment).
        [ForeignKey("Post")]
        public int? PostId { get; set; }

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

}
