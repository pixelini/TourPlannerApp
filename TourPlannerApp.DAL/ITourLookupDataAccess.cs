using static TourPlannerApp.Models.Models.TourLookup;

namespace TourPlannerApp.DAL
{
    public interface ITourLookupDataAccess
    {
        public TourLookupItem GetTour(string from, string to);

        public byte[] GetTourImage(TourLookupItem tour);

    }
}
