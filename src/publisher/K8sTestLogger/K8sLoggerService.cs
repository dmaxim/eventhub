using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using K8sTestLogger.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace K8sTestLogger
{
    public class K8sLoggerService : IHostedService
    {
        private readonly ILogger<K8sLoggerService> _logger;
        private readonly LogConfiguration _logConfiguration;
        private const string LogPropertyName = "logDetail";
        private const string WeirdPropertyName = "account.something";

        public K8sLoggerService(ILogger<K8sLoggerService> logger, LogConfiguration logConfiguration)
        {
            _logger = logger;
            _logConfiguration = logConfiguration;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var logDetail = new LogDetail("Test User", "Test action", "First log entry");

            _logger.LogWarningMessage("Test Application Starting",LogPropertyName, logDetail);
            


            for (var i = 0; i <= _logConfiguration.IterationCount; i++)
            {
                Thread.Sleep(_logConfiguration.Delay);
                logDetail.Message = $"Logging with index {i}";

                
                _logger.LogInformationMessage($"Logging from the K8s logger a message with counter {i} ", LogPropertyName, logDetail);
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
                _logger.LogWarningMessage($"Logging from the K8s logger a message with counter {logCount} ", LogPropertyName, logDetail);
            }

            if (logCount % 200 == 0)
            {
                LogException(logDetail, logCount);
                LogAdditionalInformation(logCount);
                LogAdditionalDetailWithWeirdProperty(logCount);
            }

        }

        private void LogAdditionalDetailWithWeirdProperty(int logCount)
        {
            var additionalDetail = new LogDetail("Test User", "Detail Logging", "***************Logging Detail With Property**************");

            additionalDetail.AdditionalInformation = new Dictionary<string, object>
            {
                {
                    "Envelope",
                    new Envelope
                    {
                        EnvelopeId = 122, CreateDate = DateTime.Now, CreatedBy = "Test Account",
                        SubmissionId = Guid.NewGuid().ToString()
                    }
                },
                {"Form", new Form {FormId = 334, FormName = "Test Form", FormType = "Some Type"}}
            };

            _logger.LogWarningMessage($"Logging from the K8s logger a message with counter {logCount} ", WeirdPropertyName,
                additionalDetail);
        }

        private void LogAdditionalInformation(int logCount)
        {
            var additionalDetail = new LogDetail("Test User", "Detail Logging", "***************Logging Detail**************");

            additionalDetail.AdditionalInformation = new Dictionary<string, object>
            {
                {
                    "Envelope",
                    new Envelope
                    {
                        EnvelopeId = 122, CreateDate = DateTime.Now, CreatedBy = "Test Account",
                        SubmissionId = Guid.NewGuid().ToString()
                    }
                },
                {"Form", new Form {FormId = 334, FormName = "Test Form", FormType = "Some Type"}}
            };

            _logger.LogWarningMessage($"Logging from the K8s logger a message with counter {logCount} ", LogPropertyName,
                additionalDetail);
        }

        private void LogException(LogDetail logDetail, int logCount)
        {
            try
            {
                var input = 0;
                var testing = 100 / input;
            }
            catch (Exception ex)
            {
                _logger.LogErrorMessage(ex, $"Logging from the K8s logger a message with counter {logCount}", LogPropertyName,
                    logDetail);
            }
        }
    }
}
