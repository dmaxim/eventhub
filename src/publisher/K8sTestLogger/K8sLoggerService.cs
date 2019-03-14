using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace K8sTestLogger
{
    public class K8sLoggerService : IHostedService
    {

        private readonly K8sLogger _k8sLogger;

        public K8sLoggerService(ILogger<K8sLoggerService> logger, LogConfiguration logConfiguration)
        {
            _k8sLogger = new K8sLogger(logger, logConfiguration);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _k8sLogger.GenerateLogEntries();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


    }
}
