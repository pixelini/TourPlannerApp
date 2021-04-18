using System.Collections.Generic;
using TourPlannerApp.Models;

namespace TourPlannerApp.DAL
{
    public interface ITourDataAccess
    {
        public List<TourItem> GetAllTours();

        public int AddTour(TourItem tourItem);

        public bool UpdateTour(TourItem tourItem);

        public bool DeleteTour(TourItem tourItem);

        List<TourItem> SearchByName(string tourName);

        public bool Exists(TourItem tourItem);
        
    }
}