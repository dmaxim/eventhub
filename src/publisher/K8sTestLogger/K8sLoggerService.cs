﻿using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace K8sTestLogger
{
	public class K8sLoggerService : IHostedService
	{
		private readonly ILogger<K8sLoggerService> _logger;
		public K8sLoggerService(ILogger<K8sLoggerService> logger)
		{
			_logger = logger;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogWarning("Test Application Starting");


			for (var i = 0; i < 10; i++)
			{
				Thread.Sleep(1000);

				_logger.LogWarning($"Logging from the K8s logger a message with counter {i}");
			}

			return Task.CompletedTask;
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}
	}
}