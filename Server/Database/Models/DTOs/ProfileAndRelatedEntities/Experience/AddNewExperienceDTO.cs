using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.ProfileAndRelatedEntities.Experience
{
    public class AddNewExperienceDTO
    {
        [Required(ErrorMessage = "Title is requeried")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Company is requeried")]
        public string Company { get; set; }

        public string? Location { get; set; }

        [Required(ErrorMessage = "From(Date) is requeried")]
        public DateTime From { get; set; }

        public DateTime? To { get; set; }

        public bool Current { get; set; } = false;

        public string? Description { get; set; }

    }
}
