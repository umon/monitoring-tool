using SendGrid;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MonitoringApp.Services
{
    public class SenderGridMailService : IMailService
    {
        private string ApiKey;
        private string FromMail;
        private Dictionary<string, string> BodyTags;

        public SenderGridMailService(string apiKey, string fromMail)
        {
            BodyTags = new Dictionary<string, string>();
            ApiKey = apiKey;
            FromMail = fromMail;
        }

        public async Task SendMail(string to, string subject, string plainTextContent = "", string htmlContent = "")
        {
            var client = new SendGridClient(ApiKey);
            var fromEmailAddress = new EmailAddress(FromMail, "Monitoring Tool");
            var toEmailAddress = new EmailAddress(to);

            foreach (var bodyTag in BodyTags)
            {
                htmlContent = htmlContent.Replace(bodyTag.Key, bodyTag.Value);
            }

            var msg = MailHelper.CreateSingleEmail(fromEmailAddress, toEmailAddress, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public void AddTag(string key, string value)
        {
            BodyTags[key] = value;
        }
    }
}
