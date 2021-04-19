using IISAppPoolInfo.Core.Services;
using IISAppPoolInfo.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Windows.Forms;

namespace IISAppPoolInfo.Forms
{
    public class Startup
    {
        public static ServiceProvider ServiceProvider;
        public static IConfigurationRoot ConfigurationRoot;

        public static void RegisterServices()
        {
            // configure services
            var services = new ServiceCollection()
                .AddScoped<IIISAppPoolService, IISAppPoolService>();

            // configure logger
            services
                .AddLogging(configure =>
                {
                    configure.AddNLog("nlog.config");
                })
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Trace);

            // add configuration
            ConfigurationRoot = new ConfigurationBuilder()
                .AddJsonFile(Path.GetFullPath("appsettings.json"), true, true)
                //.AddUserSecrets<Program>()
                .Build();

            ServiceProvider = services.BuildServiceProvider();
        }

        public static void SetupErrorLogger()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
        }

        static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs exception)
        {
            var _logger = Startup.ServiceProvider.GetService<ILogger>();
            _logger.LogError(exception.ExceptionObject as Exception, "An error has occured" + Environment.NewLine);
            _logger.LogWarning("Exiting because an error occured! Please check logs for error details.");

            MessageBox.Show("Exiting because an error occured! Please check logs for error details.");

            Environment.Exit(1);
        }
    }
}
