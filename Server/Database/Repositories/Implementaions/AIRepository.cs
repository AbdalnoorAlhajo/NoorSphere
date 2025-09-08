using Azure.Core;
using Database.Models.DTOs;
using Database.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System.Text;
using System.Text.Json;

namespace Database.Repositories.Implementaions
{
    public class AIRepository : IAIRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AIRepository(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public Task<string> ChatWithAI(GeminiRequestDTO request, string UserID)
        {
            request.contents[request.contents.Count - 1].parts[0].text = "Please keep your response under 100 words(higly important): " + request.contents[request.contents.Count - 1].parts[0].text;

            return Generate(request);
        }

        public Task<string> GeneratePost(string prompt)
        {
            return Generate(new GeminiRequestDTO
            {
                contents = new List<Message> 
                { 
                    new Message
                    {
                    role = "user",
                    parts = new List<Part>
                        {
                            new Part
                            {
                                text =  "Write a single social media post under 100 words. Do not ask questions. Do not include multiple options. Just generate one complete post. Topic: " + prompt
                            }
                        }
                   } }
            });
        }

        public async Task<string> Generate(GeminiRequestDTO requestBody)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                var response = await client.PostAsync(
                    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent?key={_configuration["Gemini:ApiKey"]}",
                    content
                );

                var jsonString = await response.Content.ReadAsStringAsync();
                var geminiResponse = JsonSerializer.Deserialize<GeminiRespond>(jsonString);

                if (geminiResponse != null && geminiResponse.candidates != null)
                    return geminiResponse.candidates[0].content.parts[0].text;
                else
                    return "No reponse";
            }
            catch (Exception ex)
            {
                throw ;
            }

        }
    }
}
