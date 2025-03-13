using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.ProfileAndRelatedEntities.Experience
{
    public class GetExperienceDTO
    {
        public string Title { get; set; }

        public string Company { get; set; }

        public string? Location { get; set; }

        public DateTime From { get; set; }

        public DateTime? To { get; set; }

        public bool Current { get; set; } = false;

        public string? Description { get; set; }

        public int ProfileId { get; set; }
    }
}
