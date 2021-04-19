using IISAppPoolInfo.Core.Models;
using IISAppPoolInfo.Core.Services;
using IISAppPoolInfo.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace IISAppPoolInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckAdmin.RequireAdministrator();

            Startup.RegisterServices();
            Startup.SetupErrorLogger();

            // parse args
            // - first position - what to get?
            bool appPools = (args != null & args.Length > 0 && args[0] == "-a");
            bool workerProcesses = (args != null & args.Length > 0 && args[0] == "-p");
            // -- second position - filter
            string filter = (args != null & args.Length > 1) ? args[1] : null;
            
            var _config = Startup.ConfigurationRoot.Get<AppSettings>();
            var _logger = Startup.ServiceProvider.GetService<ILogger<Program>>();
            var _iisService = Startup.ServiceProvider.GetService<IIISAppPoolService>();

            try
            {
                if (appPools)
                {
                    foreach (var appPool in _iisService.GetApplicationPools(filter))
                    {
                        Console.WriteLine($"App Pool: {appPool.Name} - State: {appPool.StateName} - Start Mode: {appPool.StartMode} - AutoStart: {appPool.AutoStart}");
                    }
                }

                if (workerProcesses)
                {
                    foreach (var workerProcess in _iisService.GetWorkerProcesses(filter))
                    {
                        Console.WriteLine($"Worker Process: {workerProcess.AppPoolName} - ProcessId: {workerProcess.ProcessId}");
                    }
                }
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Error querying IIS");
                Console.WriteLine("Error occured. Check error files.");
            }

            Startup.ServiceProvider.Dispose();
        }
    }
}
