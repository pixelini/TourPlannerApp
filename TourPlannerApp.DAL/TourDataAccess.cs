using System.Collections.Generic;
using TourPlannerApp.Models;

namespace TourPlannerApp.DAL
{
    public class TourDataAccess : ITourDataAccess
    {
        private string connectionString;

        public TourDataAccess()
        {
            connectionString = SettingsManager.GetSettings().Connection;
        }
        
        
        public List<TourItem> GetAllTours()
        {
            throw new System.NotImplementedException();
        }

        public int AddTour(TourItem tourItem)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateTour(TourItem tourItem)
        {
            throw new System.NotImplementedException();
        }

        public List<TourItem> Search(string tourName)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteTour(TourItem tourItem)
        {
            return false;
        }

        public bool Exists(TourItem tourItem)
        {
            throw new System.NotImplementedException();
        }
    }
}