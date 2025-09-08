using Database.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Repositories.Interfaces
{
    public interface IAIRepository
    {
        Task<string> GeneratePost(string prompt);

        Task<string> ChatWithAI(GeminiRequestDTO request, string UserID);

        Task<string> Generate(GeminiRequestDTO request);
    }
}
