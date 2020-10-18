using MonitoringApp.Models;
using System;
using System.Threading.Tasks;

namespace MonitoringApp.Services
{
    public interface INotificationService
    {
        Task SendNotification(TargetApplication targetApplication, DateTime requestDateTime, DateTime responseDateTime);
    }
}
