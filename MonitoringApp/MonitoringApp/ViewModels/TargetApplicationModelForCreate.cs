using System;
using System.ComponentModel.DataAnnotations;

namespace MonitoringApp.ViewModels
{
    public class TargetApplicationModelForCreate
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int Interval { get; set; }
        [Required]
        [Url]
        public string Url { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string NotificationMail { get; set; }
    }
}
