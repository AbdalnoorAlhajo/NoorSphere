using Database.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Database.Repositories.Implementaions
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        public string GetAiApiKey()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "./"))
            .AddJsonFile("appsettings.json")
            .Build();

            return configuration["Gemini:ApiKey"] ?? "False Key";
        }
    }

}
