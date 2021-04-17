using System.Collections.Generic;
using TourPlannerApp.Models;

namespace TourPlannerApp.DAL
{
    public interface ITourDataAccess
    {
        // Get all tours
        public List<TourItem> GetAllTours();

        // Add new tour
        public int AddTour(TourItem tourItem);

        // Edit tour
        public bool UpdateTour(TourItem tourItem);

        // Search in tours
        List<TourItem> Search(string tourName);

        // Delete tour
        public bool DeleteTour(TourItem tourItem);
        
        public bool Exists(TourItem tourItem);
        
    }
}