using System.Collections.Generic;

namespace AppDevMonitor
{
    public class AppleService
    {
        public AppleService ()
        {
            events = new List<AppleEvent>();
        }
        public string redirectUrl { get; set; }
        public string serviceName { get; set; }
        public List<AppleEvent> events { get; set; }
    }
}
