using System;

namespace TourPlannerApp.BL.Exceptions
{
    public class TourCollisionException : Exception
    {
        public string TourName { get; set; }

        public TourCollisionException(string message) : base(message)
        {
        }

        public TourCollisionException(string message, string tourName) : this(message)
        {
            TourName = tourName;
        }
    }
}