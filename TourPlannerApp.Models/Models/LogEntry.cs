using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlannerApp.Models
{
    public class LogEntry
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public float Distance { get; set; }
        public float Altitude { get; set; }
        public TimeSpan OverallTime { get; set; }
        public int Rating { get; set; }
    }
}
