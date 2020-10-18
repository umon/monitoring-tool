using MonitoringApp.Enums;
using System;

namespace MonitoringApp.Models
{
    public class TargetApplication : BaseEntity
    {
        public string Name { get; set; }
        public int Interval { get; set; }
        public string Url { get; set; }
        public MonitorStatus Status { get; set; }
        public string NotificationMail { get; set; }
        public Guid UserGuid { get; set; }
    }
}
