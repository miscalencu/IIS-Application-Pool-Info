using IISAppPoolInfo.Core.Models;
using IISAppPoolInfo.Core.Services;
using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;

namespace IISAppPoolInfo.Implementations
{
    public class IISAppPoolService : IIISAppPoolService
    {
        public IEnumerable<IISAppPool> GetApplicationPools(string filter = null)
        {
            using ServerManager serverManager = new();
            Configuration config = serverManager.GetApplicationHostConfiguration();
            ConfigurationSection applicationPoolsSection = config.GetSection("system.applicationHost/applicationPools");
            ConfigurationElementCollection applicationPoolsCollection = applicationPoolsSection.GetCollection();

            foreach (var appPool in applicationPoolsCollection)
            {
                appPool.GetCollection();
                if (String.IsNullOrEmpty(filter) || appPool.Attributes["name"].ToString().Contains(filter, StringComparison.InvariantCultureIgnoreCase))
                {
                    yield return new IISAppPool()
                    {
                        Name = appPool.GetAttributeValue("name").ToString(),
                        AutoStart = Boolean.Parse(appPool.GetAttributeValue("autoStart").ToString()),
                        StartMode = appPool.GetAttributeValue("startMode").ToString(),
                        State = appPool.GetAttributeValue("state").ToString()
                    };
                }
            }
        }

        public IEnumerable<IISWorkerProcess> GetWorkerProcesses(string filter = null)
        {
            using ServerManager serverManager = new();
            foreach (WorkerProcess proc in serverManager.WorkerProcesses)
            {
                if (String.IsNullOrEmpty(filter) || proc.AppPoolName.Contains(filter, StringComparison.InvariantCultureIgnoreCase))
                {
                    yield return new IISWorkerProcess()
                    {
                        ProcessId = proc.ProcessId,
                        AppPoolName = proc.AppPoolName,
                        ProcessGuid = Guid.Parse(proc.ProcessGuid),
                        State = proc.State.ToString(),
                    };
                }
            }
        }
    }
}
