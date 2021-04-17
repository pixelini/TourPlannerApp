using System.Collections.Generic;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;

namespace TourPlannerApp.BL.Services
{
    public class TourService : ITourService
    {
        private ITourDataAccess _tourDataAccess;

        public TourService(ITourDataAccess tourDataAccess)
        {
            _tourDataAccess = tourDataAccess;
        }

        public List<TourItem> GetAllTours()
        {
            return _tourDataAccess.GetAllTours();
        }

        public int AddTour(TourItem tourItem)
        {
            throw new System.NotImplementedException();
        }

        public bool UpdateTour(TourItem tourItem)
        {
            throw new System.NotImplementedException();
        }

        public bool DeleteTour(TourItem tourItem)
        {
            // find by id
            if (_tourDataAccess.Exists(tourItem))
            {
                return false;
            }

            return _tourDataAccess.DeleteTour(tourItem);
        }

        public List<TourItem> SearchByName(string tourName)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(TourItem tourItem)
        {
            throw new System.NotImplementedException();
        }
    }
}