using System.Collections.Generic;

namespace AppDevMonitor
{
    public class AppleServiceRoot
    {
        public AppleServiceRoot()
        {
            services = new List<AppleService>();
        }

        public bool drpost { get; set; }
        public object drMessage { get; set; }
        public List<AppleService> services { get; set; }
    }
}
