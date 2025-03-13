using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.ProfileAndRelatedEntities.Education
{
    public class GetEducationDTO
    {
        public string School { get; set; }

        public string Degree { get; set; }

        public string FieldOfStudy { get; set; }

        public DateTime From { get; set; }

        public DateTime? To { get; set; }

        public bool Current { get; set; } = false;

        public string? Description { get; set; }

        public int ProfileId { get; set; }
    }
}
