using TourPlannerApp.Models;

namespace TourPlannerApp.BL.Services
{
    public interface ITourLookupService
    {
        public TourItem GetTour(string from, string to);
    }
}