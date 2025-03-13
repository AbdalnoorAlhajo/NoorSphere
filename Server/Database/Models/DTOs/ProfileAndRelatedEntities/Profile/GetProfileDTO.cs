using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.ProfileAndRelatedEntities.Profile
{
    public class GetProfileDTO
    {
        public int UserId { get; set; }
        public string? Company { get; set; }
        public string? Website { get; set; }
        public string? Country { get; set; }
        public string? Location { get; set; }

        public string Status { get; set; }

        public string Skills { get; set; }

        public string? Bio { get; set; }

        public DateTime? Date { get; set; }
    }
}
