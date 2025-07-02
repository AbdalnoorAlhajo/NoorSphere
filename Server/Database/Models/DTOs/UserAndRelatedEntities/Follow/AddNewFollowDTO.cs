using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.UserAndRelatedEntities.Follow
{
    public class AddNewFollowDTO
    {
        public string FollowedUserId { get; set; }
        public string FollowerUserId { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
