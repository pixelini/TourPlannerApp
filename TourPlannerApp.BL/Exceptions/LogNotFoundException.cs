using System;

namespace TourPlannerApp.BL.Exceptions
{
    public class LogNotFoundException : Exception
    {
        public string TourName { get; set; }

        public LogNotFoundException(string message) : base(message)
        {
        }

        public LogNotFoundException(string message, string tourName) : this(message)
        {
            TourName = tourName;
        }
    }
}
