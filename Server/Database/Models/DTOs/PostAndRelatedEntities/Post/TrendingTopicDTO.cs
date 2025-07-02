using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs.PostAndRelatedEntities.Post
{
    public class TrendingTopicDTO
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public int Count { get; set; }
    }
}
