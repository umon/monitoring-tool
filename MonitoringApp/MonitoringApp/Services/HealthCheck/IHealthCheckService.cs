using MonitoringApp.Models;
using System.Threading.Tasks;

namespace MonitoringApp.Services
{
    public interface IHealthCheckService
    {
        Task CheckApp(TargetApplication targetApplication);
        void ClearJobs();
        void AddJob(TargetApplication targetApplication);
        void RemoveJob(string jobId);
    }
}
