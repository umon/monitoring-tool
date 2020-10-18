using Hangfire;
using Hangfire.Storage;
using MonitoringApp.Models;
using MonitoringApp.Services.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MonitoringApp.Services
{
    public class HealthCheckService : IHealthCheckService
    {

        private readonly INotificationService notificationService;

        public HealthCheckService(INotificationService notificationService)
        {
            this.notificationService = notificationService;
        }

        public async Task CheckApp(TargetApplication targetApplication)
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var requestDateTime = DateTime.Now;
                    var uri = new Uri(targetApplication.Url);
                    var response = await client.GetAsync(uri);
                    var responseStatusCode = (int)response.StatusCode;
                    var responseDateTime = DateTime.Now;

                    if (responseStatusCode < 200 || responseStatusCode >= 300)
                    {
                        await notificationService.SendNotification(targetApplication, requestDateTime, responseDateTime);
                    }
                }
                catch (Exception exc)
                {
                    LogService.LogError($"\n Exception: \n {exc} ");
                }
            }
        }

        public void ClearJobs()
        {
            using (var storageConnection = JobStorage.Current.GetConnection())
            {
                foreach (var recurringJob in storageConnection.GetRecurringJobs())
                {
                    RecurringJob.RemoveIfExists(recurringJob.Id);
                }
            }
        }

        public void AddJob(TargetApplication targetApplication)
        {
            RecurringJob.AddOrUpdate(targetApplication.Id.ToString(), () => CheckApp(targetApplication), $"*/{targetApplication.Interval} * * * * ");
        }

        public void RemoveJob(string jobId)
        {
            RecurringJob.RemoveIfExists(jobId);

        }
    }
}
