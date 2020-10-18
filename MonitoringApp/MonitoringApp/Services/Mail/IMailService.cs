using System.Threading.Tasks;

namespace MonitoringApp.Services
{
    public interface IMailService
    {
        Task SendMail(string to, string subject, string plainTextContent = "", string htmlContent = "");
        void AddTag(string key, string value);
    }
}
