using System.Collections.Generic;
using System.Diagnostics;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;
using static TourPlannerApp.Models.Models.TourLookup;

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

        public int AddTour(TourItem newTourItem)
        {
            if (!Exists(newTourItem))
            {
                int tourId = _tourDataAccess.AddTour(newTourItem);
                if (tourId >= 0)
                {
                    Debug.WriteLine("Tour was successfully added.");
                    return tourId;
                }

                Debug.WriteLine("Tour couldn't be added.");
                return tourId;
            }

            Debug.WriteLine("Tour already exits.");
            
            return -1;
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

        //private TourItem ConvertTourLookupToTourItem(TourLookupItem tourLookupItem, string name, string startLocation, string targetLocation)
        //{
        //    TourItem tour = new TourItem();
        //    tour.Name = name;
        //    tour.StartLocation = startLocation;
        //    tour.TargetLocation = targetLocation;
        //    tour.Distance = tourLookupItem.Route.Distance;
        //    tour.NavigationDetails = new List<string>();

        //    foreach (var maneuver in tourLookupItem.Route.Legs[0].Maneuvers)
        //    {
        //        tour.NavigationDetails.Add(maneuver.Narrative);
        //    }

        //    return tour;
           
        //}

    }
}