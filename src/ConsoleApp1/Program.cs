using System;
using System.IO;

using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

using Shuvava.Extensions.Logging.ApplicationInsights;


namespace ConsoleApp1
{
    internal class Program
    {
        private static string key = "0f4f2c92-b932-4a00-beb7-942c2357c9b6";
        private static int run = 2;
        private static void Main(string[] args)
        {
            Console.WriteLine("Application Started!");

            var services = new ServiceCollection();
            ConfigureServices(services);
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<Program>>();
            var telemetryClient = serviceProvider.GetService<TelemetryClient>();
            logger.LogWarning($"test{run}");
            logger.LogInformation($"test{run}Information");
            logger.LogWarning($"test{run}");
            logger.LogInformation($"test{run}Information");
            var sample = new MetricTelemetry();
            sample.Name = "metric name";
            sample.Count = 1;
            telemetryClient.TrackMetric(sample);
            logger.LogMetric($"Metric{run}", 1);
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }


        private static void ConfigureServices(ServiceCollection services)
        {
            var configuration = GetConfiguration();
            services.AddSingleton(new LoggerFactory());
            var telemetryClient = new TelemetryClient();
            telemetryClient.InstrumentationKey = key;
            services.AddSingleton(telemetryClient);

            services
                .AddLogging(loggingBuilder =>
                {
                    loggingBuilder
                        .AddConsole()
                        .AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace)
                        .AddApplicationInsights(key)
                        //.AddConfiguration(configuration.GetSection("Logging"))
                        ;
                }); // Allow ILogger<T>




            services.AddSingleton(configuration);
        }


        private static IConfigurationRoot GetConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", true)
                .Build();
        }
    }
}
