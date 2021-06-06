using System;

namespace TourPlannerApp.BL.Exceptions
{
    public class TourNotFoundException : Exception
    {
        public string TourName { get; set; }

        public TourNotFoundException(string message) : base(message)
        {
        }

        public TourNotFoundException(string message, string tourName) : this(message)
        {
            TourName = tourName;
        }
    }
}