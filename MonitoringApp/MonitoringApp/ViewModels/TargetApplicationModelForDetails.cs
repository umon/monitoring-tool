using MonitoringApp.Enums;

namespace MonitoringApp.ViewModels
{
    public class TargetApplicationModelForDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Interval { get; set; }
        public MonitorStatus Status { get; set; }
        public string Url { get; set; }
        public string NotificationMail { get; set; }
    }
}
