using System.Collections.Generic;
using TourPlannerApp.Models;

namespace TourPlannerApp.BL.Services
{
    public interface ITourService
    {
        // Get all tours
        public List<TourItem> GetAllTours();

        // Add new tour
        public int AddTour(TourItem tourItem);

        // Edit tour
        public bool UpdateTour(TourItem tourItem);

        // Delete tour
        public bool DeleteTour(TourItem tourItem);
        
        // Search in tours
        List<TourItem> SearchByName(string tourName);
        
        public bool Exists(TourItem tourItem);

    }
}