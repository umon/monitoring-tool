using System;
using System.Text.Json;

namespace MonitoringApp.Services.Logging
{
    public class LogModel
    {
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, typeof(LogModel), new JsonSerializerOptions { WriteIndented = true });
        }
    }
}
