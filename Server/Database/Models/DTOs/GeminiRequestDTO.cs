using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.DTOs
{
    public class GeminiRespond
    {
        public List<Candidate> candidates { get; set; }
    }

    public class Candidate
    {
        public Message content { get; set; }
    }
    public class GeminiRequestDTO
    {
        public List<Message> contents { get; set; }
    }

    public class Message
    {
        public string role { get; set; }
        public List<Part> parts { get; set; }
    }

    public class Part
    {
        public string text { get; set; }
    }

}
