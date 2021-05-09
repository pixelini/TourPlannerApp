using System.Threading.Tasks;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;
using TourPlannerApp.Models.Models;
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

        public TourItem GetTour(string from, string to)
        {
            var tourLookup = _tourLookupDataAccess.GetTour(from, to);

            // if tour lookup successful
            if (tourLookup != null)
            {
                var image = GetTourImage(tourLookup);
                var tourItem = TourLookup.ConvertTourLookupToTourItem(tourLookup);

                if (image != null)
                {
                    tourItem.Image = image;
                    return tourItem;
                } else
                {
                    // tour item without image is returned
                    return tourItem;
                }
            }

            return null;
        }

        private byte[] GetTourImage(TourLookupItem tour)
        {
            return _tourLookupDataAccess.GetTourImage(tour);
        }
    }
}