using System.IO;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;


namespace K8sTestLogger
{
	public class Program
	{
        public static void Main(string[] args)
        {

            //var hostBuilder = new HostBuilder()
            //    .ConfigureHostConfiguration(hostConfig => { hostConfig.SetBasePath(Directory.GetCurrentDirectory()); })
            //    .ConfigureAppConfiguration((hostingContext, appConfig) =>
            //    {
            //        appConfig.AddJsonFile("appsettings.json", false)
            //            .AddJsonFile("environment/appsettings.json", true)
            //            .AddEnvironmentVariables();
            //    })
            //    .ConfigureLogging((hostContext, loggingBuilder) =>
            //    {
            //        //var eventHubClient = GetEventHubClient(hostContext.Configuration);
            //        //var test = new JsonFormatter();
            //        Log.Logger =
            //            new LoggerConfiguration()
            //                .ReadFrom.Configuration(hostContext.Configuration)
            //              //   .WriteTo.Sink(new AzureEventHubSink(eventHubClient: eventHubClient, new JsonFormatter()))
            //                .CreateLogger();
            //        loggingBuilder.AddSerilog();

            //    })
            //    .ConfigureServices((hostContext, collection) =>
            //    {
            //        var logConfiguration = GetConfiguration(hostContext.Configuration);

            //        collection.AddSingleton<LogConfiguration>((context) => logConfiguration);

            //        collection.AddSingleton<IHostedService, K8sLoggerService>();

            //    }).UseConsoleLifetime();

            //await hostBuilder.RunConsoleAsync();


            //Log.CloseAndFlush();

            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("environment/appsettings.json", true)
                .AddEnvironmentVariables();

            var config = configBuilder.Build();

           Log.Logger =
                new LoggerConfiguration()
                    .ReadFrom.Configuration(config)
                .CreateLogger();

            var services = new ServiceCollection();

            services.AddLogging(builder => builder.AddSerilog());

            var serviceProvider = services.BuildServiceProvider();

            var k8sLogger = new K8sLogger(serviceProvider.GetService<ILogger<K8sLogger>>(), GetConfiguration(config));

            k8sLogger.GenerateLogEntries();
        }


        private static LogConfiguration GetConfiguration(IConfiguration configuration)
        {
            
            var logConfiguration = new LogConfiguration();
            configuration.GetSection("LogConfiguration").Bind(logConfiguration);

            return logConfiguration;

        }

        private static EventHubClient GetEventHubClient(IConfiguration configuration)
        {
            var eventHubConfiguration = new LoggingEventHubConfiguration();
            configuration.GetSection("LoggingEventHub").Bind(eventHubConfiguration);

            var client =  EventHubClient.CreateFromConnectionString(eventHubConfiguration.ConnectionString);

            return client;

        }


		
	}
}
