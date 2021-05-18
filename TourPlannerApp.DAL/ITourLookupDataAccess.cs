using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.Models;
using static TourPlannerApp.Models.Models.TourLookup;

namespace TourPlannerApp.DAL
{
    public interface ITourLookupDataAccess
    {
        public TourLookupItem GetTour(string from, string to);

        public byte[] GetTourImage(TourLookupItem tour);

    }
}
