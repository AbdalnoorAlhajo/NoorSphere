using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace Database.Models.Domain
{
    public class User : IdentityUser
    {
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime? LastUpdated { get; set; }
    }
}
