using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace K8sTestLogger
{
	public class Program
	{
		public static async Task Main(string[] args)
		{

			//var hostBuilder = new HostBuilder()
			//	.ConfigureHostConfiguration(hostConfig => { hostConfig.SetBasePath(Directory.GetCurrentDirectory()); })
			//	.ConfigureAppConfiguration((hostingContext, appConfig) =>
			//	{
			//		appConfig.AddJsonFile("appsettings.json", false).AddEnvironmentVariables();
			//	})
			//	.ConfigureLogging((hostContext, loggingBuilder) =>
			//	{

			//		Log.Logger =
			//			new LoggerConfiguration()
			//				.ReadFrom.Configuration(hostContext.Configuration)
			//				.CreateLogger();
			//		loggingBuilder.AddSerilog();
			//	})
			//	.ConfigureServices((hostContext, collection) =>
			//		{
			//			collection.AddSingleton<IHostedService, K8sLoggerService>();

			//		}).UseConsoleLifetime();

			//await hostBuilder.RunConsoleAsync();

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


		
	}
}
