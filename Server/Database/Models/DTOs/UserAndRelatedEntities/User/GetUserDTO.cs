using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.UserAndRelatedEntities.User
{
    public class GetUserDTO
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        public DateTime? LastUpdated { get; set; }
    }
}
