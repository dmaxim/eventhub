using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace K8sTestLogger
{
    public static class LoggingExtensions
    {

        public static IDisposable BeginDestructuredScope(this ILogger logger, string propertyName, object state)
        {
            var placeHolder = "{@" + propertyName + "}";
            return logger.BeginScope(placeHolder, state);

        }

        public static void WithDestructuredScope(this ILogger logger, string propertyName, object state, Action<ILogger> action)
        {
            var placeHolder = "{@" + propertyName + "}";

            using (logger.BeginScope(placeHolder, state))
            {
                action(logger);
            }


        }

        public static void LogErrorMessage(this ILogger logger, Exception ex, string message, string propertyName, object state)
        {
            WithDestructuredScope(logger, propertyName, state, messageLogger => messageLogger.LogError(ex, message));

            
        }

        public static void LogInformationMessage(this ILogger logger, string message, string propertyName, object state)
        {
            WithDestructuredScope(logger, propertyName, state, messageLogger => messageLogger.LogInformation(message));
        }

        public static void LogWarningMessage(this ILogger logger, string message, string propertyName, object state)
        {
            WithDestructuredScope(logger, propertyName, state, messageLogger => messageLogger.LogWarning(message));
        }


    }
}
