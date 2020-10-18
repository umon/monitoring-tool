using MonitoringApp.Models;
using System;
using System.Threading.Tasks;

namespace MonitoringApp.Services
{
    public class MailNotificationService : INotificationService
    {
        private readonly IMailService mailService;

        public MailNotificationService(IMailService mailService)
        {
            this.mailService = mailService;
        }

        public async Task SendNotification(TargetApplication targetApplication, DateTime requestDateTime, DateTime responseDateTime)
        {
            mailService.AddTag("[[TargetAppName]]", targetApplication.Name);
            mailService.AddTag("[[TargetAppUrl]]", targetApplication.Url);
            mailService.AddTag("[[TargetAppRequest]]", requestDateTime.ToString("HH:mm:ss dd.MM.yyyy"));
            mailService.AddTag("[[TargetAppResponse]]", responseDateTime.ToString("HH:mm:ss dd.MM.yyyy"));

            var plainTextContent = "Application is Down!";
            var htmlContent =
                   "<br />APPLICATION IS DOWN!!!<br /><br /><br />" +
                   "Target App. Name: <strong>[[TargetAppName]]</strong><br />" +
                   "Target App. Url: <strong>[[TargetAppUrl]]</strong><br />" +
                   "Target App. Request Time: <strong>[[TargetAppRequest]]</strong><br />" +
                   "Target App. Response Time: <strong>[[TargetAppResponse]]</strong><br />";

            await mailService.SendMail(
                targetApplication.NotificationMail, 
                $"Application is down: {targetApplication.Name} ({targetApplication.Url})",
                plainTextContent,
                htmlContent);
        }
    }
}
