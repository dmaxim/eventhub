using System;
using System.IO;
using System.Threading;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace K8sTestLogger
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddEnvironmentVariables();

			var config = builder.Build();

			var logger = new LoggerConfiguration()
				.ReadFrom.Configuration(config)
				.CreateLogger();

			logger.Warning("Test Application Starting");


			for (var i = 0; i < 10; i++)
			{
				Thread.Sleep(1000);

				logger.Warning($"Logging a message with counter {i}");
			}

			Thread.Sleep(5000);
		}
	}
}
