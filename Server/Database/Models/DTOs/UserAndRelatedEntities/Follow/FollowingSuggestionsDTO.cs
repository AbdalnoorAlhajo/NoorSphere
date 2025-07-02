using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.UserAndRelatedEntities.Follow
{
    public class FollowingSuggestionsDTO
    {
        public string UserId { get; set; }

        public string? AvatarUrl { get; set; }

        public string Name {  get; set; }

    }
}
