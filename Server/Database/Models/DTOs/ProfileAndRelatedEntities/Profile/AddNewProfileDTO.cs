using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.ProfileAndRelatedEntities.Profile
{
    public class AddNewProfileDTO
    {
        public string? Company { get; set; }
        public string? Website { get; set; }
        public string? Country { get; set; }
        public string? Location { get; set; }
        public string? AvatarUrl { get; set; }

        [Required(ErrorMessage = "Status is Required")]
        public string Status { get; set; }

        [Required(ErrorMessage = "Skills is Required")]
        public string Skills { get; set; }

        public string? Bio { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
