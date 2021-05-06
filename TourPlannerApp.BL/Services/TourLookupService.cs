using System.Threading.Tasks;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;
using static TourPlannerApp.Models.Models.TourLookup;

namespace TourPlannerApp.BL.Services
{
    public class TourLookupService : ITourLookupService
    {
        private ITourLookupDataAccess _tourLookupDataAccess;

        public TourLookupService(ITourLookupDataAccess tourLookupDataAccess)
        {
            _tourLookupDataAccess = tourLookupDataAccess;
        }

        public TourLookupItem GetTour(string from, string to)
        {
            return _tourLookupDataAccess.GetTour(from, to);
        }

        public byte[] GetTourImage(TourLookupItem tour)
        {
            return _tourLookupDataAccess.GetTourImage(tour);
        }
    }
}