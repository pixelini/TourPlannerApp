using System.Threading.Tasks;
using TourPlannerApp.Models;
using static TourPlannerApp.Models.Models.TourLookup;

namespace TourPlannerApp.BL.Services
{
    public interface ITourLookupService
    {
        public TourLookupItem GetTour(string from, string to);

        public byte[] GetTourImage(TourLookupItem tour);
    }
}