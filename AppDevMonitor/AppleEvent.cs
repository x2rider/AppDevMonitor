namespace AppDevMonitor
{
    public class AppleEvent
    {
        public string usersAffected { get; set; }
        public long epochStartDate { get; set; }
        public long epochEndDate { get; set; }
        public string messageId { get; set; }
        public string statusType { get; set; }
        public string datePosted { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public object affectedServices { get; set; }
        public string eventStatus { get; set; }
        public string message { get; set; }
    }
}