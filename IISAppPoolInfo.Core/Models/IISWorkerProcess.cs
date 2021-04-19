using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IISAppPoolInfo.Core.Models
{
    public class IISWorkerProcess
    {
        public int ProcessId { get; set; }

        public Guid ProcessGuid { get; set; }

        public string AppPoolName { get; set; }


        public string State { get; set; }
    }
}
