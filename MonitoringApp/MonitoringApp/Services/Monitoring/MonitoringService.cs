using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonitoringApp.Models;
using MonitoringApp.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MonitoringApp.Services
{
    public class MonitoringService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHealthCheckService _healthCheckService;

        public MonitoringService(IServiceProvider serviceProvider, IHealthCheckService healthCheckService)
        {
            _serviceProvider = serviceProvider;
            _healthCheckService = healthCheckService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var targetAppRepo = scope.ServiceProvider.GetRequiredService<IRepository<TargetApplication>>();

                var targetApps = await targetAppRepo.List(x => x.Status != Enums.MonitorStatus.Stopped);

                _healthCheckService.ClearJobs();

                foreach (var targetApp in targetApps)
                {
                    _healthCheckService.AddJob(targetApp);
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _healthCheckService.ClearJobs();
        }
    }
}
