using System;

namespace TourPlannerApp.Models
{
    public class LogEntry
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public float Distance { get; set; }
        public float Altitude { get; set; }
        public TimeSpan OverallTime { get; set; }
        public int Rating { get; set; }
        public int Weather { get; set; }
        public int NumberOfBreaks { get; set; }
        public int NumberOfParticipants { get; set; }
        public float AvgSpeed { get; set; }

    }
}
