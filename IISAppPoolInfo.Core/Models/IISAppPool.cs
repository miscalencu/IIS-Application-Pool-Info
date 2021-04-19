namespace IISAppPoolInfo.Core.Models
{
    public class IISAppPool
    {
        public string Name { get; set; }

        public bool AutoStart { get; set; }

        public string StartMode { get; set; }

        public string State { get; set; }

        public string StateName 
        {
            get
            {
                return this.State switch
                {
                    "1" => "Running",
                    "3" => "Stopped",
                    _ => "?",
                };
            }
        }
    }
}
