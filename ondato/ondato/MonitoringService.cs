using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ondato.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ondato.HostedService
{
    public interface IMonitoringService
    {
        Task DoWork(CancellationToken stoppingToken);
    }

    public class MonitoringService : IMonitoringService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;
        private ICacheService cacheService;
        private readonly IConfiguration configuration;
        private readonly IServiceProvider serviceProvider;

        public MonitoringService(ILogger<MonitoringService> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            this._logger = logger;
            this.configuration = configuration;
            this.serviceProvider = serviceProvider;

        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                executionCount++;

                _logger.LogInformation("Monitoring - Count: {Count}", executionCount);
                _logger.LogInformation("Cleaning Data", DateTime.Now.ToString());

                using (var scope = serviceProvider.CreateScope())
                {
                    this.cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();
                    cacheService.Cleanup();
                }
              
                int timeout = configuration.GetSection("Dictionary").GetValue<int>("CleanupInterval");

                await Task.Delay(timeout * 1000, stoppingToken);
            }
        }
    }
}