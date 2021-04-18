using System.Collections.Generic;
using System.Diagnostics;
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
            if (Exists(tourItem))
            {
                Debug.WriteLine("Tour exists.");
                // delete if tour exists
                if (_tourDataAccess.DeleteTour(tourItem))
                {
                    Debug.WriteLine("Tour was successfully deleted.");
                    return true;
                }

                Debug.WriteLine("Tour couldn't be deleted.");
                return false;
            }

            Debug.WriteLine("Tour does not exist.");
            return false;
        }

        public List<TourItem> SearchByName(string tourName)
        {


            throw new System.NotImplementedException();
        }

        public bool Exists(TourItem tourItem)
        {
            return _tourDataAccess.Exists(tourItem);
        }
    }
}