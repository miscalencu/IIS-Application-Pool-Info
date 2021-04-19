using IISAppPoolInfo.Core.Models;
using System.Collections.Generic;

namespace IISAppPoolInfo.Core.Services
{
    public interface IIISAppPoolService
    {
        /// <summary>
        /// Returns IIS Worker Processes
        /// </summary>
        /// <param name="filter">string query to filter the App Pool Name</param>
        /// <returns></returns>
        IEnumerable<IISWorkerProcess> GetWorkerProcesses(string filter = null);

        /// <summary>
        /// Returns IIS Application Pools
        /// </summary>
        /// <param name="filter">string query to filter the App Pool Name</param>
        /// <returns></returns>
        IEnumerable<IISAppPool> GetApplicationPools(string filter = null);
    }
}
