using System;

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
