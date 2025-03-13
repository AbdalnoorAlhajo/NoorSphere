using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.PostAndRelatedEntities.Comment
{
    public class AddNewCommentDTO
    {
        [Required(ErrorMessage = "UserId is required")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "PostId is required")]
        public int PostId { get; set; }

        [Required(ErrorMessage = "Text is required")]
        public string Text { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
