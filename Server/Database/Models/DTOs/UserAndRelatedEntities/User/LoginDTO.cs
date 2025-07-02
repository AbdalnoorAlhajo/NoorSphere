using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.UserAndRelatedEntities.User
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password should be at least 6 characters long")]
        public string Password { get; set; }
    }
}
