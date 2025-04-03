using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.ProfileAndRelatedEntities.Education
{
    public class AddNewEducationDTO
    {
        [Required(ErrorMessage = "School is Required")]
        public string School { get; set; }

        [Required(ErrorMessage = "Degree is Required")]
        public string Degree { get; set; }

        [Required(ErrorMessage = "FieldOfStudy is Required")]
        public string FieldOfStudy { get; set; }

        [Required(ErrorMessage = "From(Date) is Required")]
        public DateTime From { get; set; }

        public DateTime? To { get; set; }

        public bool Current { get; set; } = false;

        public string? Description { get; set; }

        public int ProfileId { get; set; }
    }
}
