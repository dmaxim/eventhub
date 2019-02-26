using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging.Configuration;
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

				logger.Warning($"Logging from the K8s logger a message with counter {i}");
			}

			Thread.Sleep(5000);
		}


		private static void StartUp()
		{
			var builder = new HostBuilder()
				.ConfigureAppConfiguration((hostingContext, config) =>
					{
						config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
						config.AddEnvironmentVariables();
					})
				.ConfigureLogging((hostingContext, logging) =>
					{
						logging.AddConfiguration();
						logging.AddSerilog();
					}
				);


		}
	}
}
