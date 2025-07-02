using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.UserAndRelatedEntities.Follow
{
    public class GetFollowDTO
    {
        public Guid Id { get; set; }

        public string FollowedUserId { get; set; }

        public string FollowerUserId { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
