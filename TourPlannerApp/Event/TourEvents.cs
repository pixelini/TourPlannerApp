using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.Models;

namespace TourPlannerApp.Store
{
    public class TourEvents
    {
        public static event Action TourCreated;

        public static void AnnounceNewTour()
        {
            TourCreated?.Invoke();
        }

    }
}
