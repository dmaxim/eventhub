using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace K8sTestLogger
{
    public class K8sLoggerService : IHostedService
    {
        private readonly ILogger<K8sLoggerService> _logger;
        private readonly LogConfiguration _logConfiguration;

        public K8sLoggerService(ILogger<K8sLoggerService> logger, LogConfiguration logConfiguration)
        {
            _logger = logger;
            _logConfiguration = logConfiguration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var logDetail = new LogDetail("Test User", "Test action", "First log entry");
            _logger.LogWarning("Test Application Starting {@logDetail} ", logDetail);


            for (var i = 0; i < _logConfiguration.IterationCount; i++)
            {
                Thread.Sleep(_logConfiguration.Delay);
                logDetail.Message = $"Logging with index {i}";
                _logger.LogInformation($"Logging from the K8s logger a message with counter {i} " + " {@logDetail}", logDetail);
                LogMessage(logDetail, i);
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        private void LogMessage(LogDetail logDetail, int logCount)
        {
            if (logCount % 20 == 0)
            {
                _logger.LogWarning($"Logging from the K8s logger a message with counter {logCount} " + " {@logDetail} ", logDetail);
            }

            if (logCount % 200 == 0)
            {
                _logger.LogError($"Logging from the K8s logger a message with counter {logCount} " + " {@logDetail} ", logDetail);
            }
        }
    }
}
