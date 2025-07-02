using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Database.Models.Domain
{
    public class User : IdentityUser
    {
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdated { get; set; }

        // Users this user is following
        public List<Follow> Followings { get; set; }

        // Users who are following this user
        public List<Follow> Followers { get; set; }
    }

    public class Follow
    {
        [Key]
        public Guid Id { get; set; }

        public string FollowedUserId { get; set; }
        public User FollowedUser { get; set; }

        public string FollowerUserId { get; set; }
        public User FollowerUser { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
