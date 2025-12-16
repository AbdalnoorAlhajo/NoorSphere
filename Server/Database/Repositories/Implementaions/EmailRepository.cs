using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Database.Repositories.Implementaions
{


    public class MailJetEmailService
    {
        private readonly IConfiguration _config;
        private readonly MailjetClient _client;

        public MailJetEmailService(IConfiguration config)
        {
            _config = config;
            _client = new MailjetClient(
                _config["MailJet:ApiKey"],
                _config["MailJet:SecretKey"]
            );
        }

        public async Task<bool> SendVerificationEmail(string toEmail, string toName, string verificationUrl)
        {
            var request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
            .Property(Send.FromEmail, _config["MailJet:FromEmail"])
            .Property(Send.FromName, _config["MailJet:FromName"])
            .Property(Send.Subject, "Verify Your Email")
            .Property(Send.HtmlPart,
                $"<h3>Welcome!</h3><p>Please verify your email:</p><a href='{verificationUrl}'>Verify Email</a>")
            .Property(Send.Recipients, new JArray {
            new JObject {
                {"Email", toEmail},
                {"Name", toName}
            }
            });

            var response = await _client.PostAsync(request);
            return response.IsSuccessStatusCode;
        }
    }

}
