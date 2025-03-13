using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.PostAndRelatedEntities.Comment
{
    public class GetCommentDTO
    {
        public string Text { get; set; }

        public string Name { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow;
    }
}
