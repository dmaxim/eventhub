
using System;
using System.Collections.Generic;
using System.Threading;
using K8sTestLogger.Model;
using Microsoft.Extensions.Logging;

namespace K8sTestLogger
{
    public class K8sLogger
    {
        private readonly ILogger<K8sLogger> _logger;
        private readonly LogConfiguration _logConfiguration;
        private const string LogPropertyName = "logDetail";
        private const string WeirdPropertyName = "account.accountId";

        public K8sLogger(ILogger<K8sLogger> logger, LogConfiguration logConfiguration)
        {
            _logger = logger;
            _logConfiguration = logConfiguration;
        }

        public void GenerateLogEntries()
        {
            var logDetail = new LogDetail("Test User", "Test action", "First log entry");

            _logger.LogWarningMessage("Test Application Starting", LogPropertyName, logDetail);



            for (var i = 0; i <= _logConfiguration.IterationCount; i++)
            {
                Thread.Sleep(_logConfiguration.Delay);
                logDetail.Message = $"Logging with index {i}";


                _logger.LogInformationMessage($"Logging from the K8s logger a message with counter {i} ", LogPropertyName, logDetail);
                LogMessage(logDetail, i);
            }

        }

        private void LogMessage(LogDetail logDetail, int logCount)
        {
            if (logCount % 20 == 0)
            {
                _logger.LogWarningMessage($"Logging from the K8s logger a message with counter {logCount} ", LogPropertyName, logDetail);
                LogAdditionalDetailWithWeirdProperty(logCount);
            }

            if (logCount % 50 == 0)
            {
                LogAdditionalDetailWithWeirdDictionary(logCount);
                LogException(logDetail, logCount);
                LogAdditionalInformation(logCount);
                
            }

        }

        private void LogAdditionalDetailWithWeirdProperty(int logCount)
        {
      
            _logger.LogInformation("***Logging Account Id Account Id: {account.accountId}", 1111 );
        }

        private void LogAdditionalDetailWithWeirdDictionary(int logCount)
        {
            var additionalDetail = new LogDetail("Test User", "Detail Logging", "***************Logging Detail With Dictionary Dot**************");

            additionalDetail.AdditionalInformation = new Dictionary<string, object>
            {
                {
                    "Envelope.My",
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
