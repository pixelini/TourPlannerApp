using TourPlannerApp.DAL;
using TourPlannerApp.Models;

namespace TourPlannerApp.BL.Services
{
    public class TourLookupService : ITourLookupService
    {

        private ITourLookupDataAccess _tourLookupDataAccess;

        public TourLookupService(ITourLookupDataAccess tourLookupDataAccess)
        {
            _tourLookupDataAccess = tourLookupDataAccess;
        }

        public TourItem GetTour(string from, string to)
        {
            return _tourLookupDataAccess.GetTour(from, to);
        }
    }
}