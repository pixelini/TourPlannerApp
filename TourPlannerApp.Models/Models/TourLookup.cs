using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static TourPlannerApp.Models.TourItem;

namespace TourPlannerApp.Models.Models
{
    public class TourLookup
    {
        public class TourLookupItem
        {
            public Info Info { get; set; }
            public Route Route { get; set; }
        }

        public class Info
        {
            public int Statuscode { get; set; }
        }

        public class Route
        {
            public RouteError RouteError { get; set; }
            public string SessionId { get; set; }
            public BoundingBox BoundingBox { get; set; }
            public List<Location> Locations { get; set; }
            public double Distance { get; set; }
            public List<Leg> Legs { get; set; }
            public Options Options { get; set; }
        }

        public class RouteError
        {
            public int ErrorCode { get; set; }
            public string Message { get; set; }
        }

        public class Location
        {
            public string AdminArea4 { get; set; } // County / District
            public string AdminArea5 { get; set; } // City
            public string PostalCode { get; set; }
            public string Street { get; set; }
            public string AdminArea1 { get; set; } // Country
            public string AdminArea3 { get; set; } // State
            public string AdminArea5Type { get; set; }
            public string AdminArea1Type { get; set; }
            public string AdminArea3Type { get; set; }
        }

        public class BoundingBox
        {
            public Lr Lr { get; set; }
            public Ul Ul { get; set; }
        }

        public class Lr
        {
            public double Lng { get; set; }
            public double Lat { get; set; }
        }

        public class Ul
        {
            public double Lng { get; set; }
            public double Lat { get; set; }
        }

        public class Leg
        {
            public List<Maneuver> Maneuvers { get; set; }
        }

        public class Maneuver
        {
            public string Narrative { get; set; }
        }

        public class Options
        {
            public string RouteType { get; set; }
        }


        public static TourItem ConvertTourLookupToTourItem(TourLookupItem tourLookupItem)
        {
            TourItem tour = new TourItem();
            //tour.Name = name;
            //tour.StartLocation = startLocation;
            //tour.TargetLocation = targetLocation;

            if (tourLookupItem.Route.Locations[0] != null && tourLookupItem.Route.Locations[1] != null)
            {
                FillTourItemWithLocationData(tour, tourLookupItem.Route.Locations);
            }

            tour.Distance = tourLookupItem.Route.Distance;
            tour.NavigationDetails = new List<string>();

            foreach (var maneuver in tourLookupItem.Route.Legs[0].Maneuvers)
            {
                tour.NavigationDetails.Add(maneuver.Narrative);
            }

            return tour;
        }

        private static void FillTourItemWithLocationData(TourItem tour, List<Location> locations)
        {
            // start location data
            var startLocation = new Address();
            startLocation.Street = locations[0].Street ??= "";
            startLocation.PostalCode = locations[0].PostalCode ??= "";
            startLocation.City = locations[0].AdminArea5 ??= "";
            startLocation.County = locations[0].AdminArea4 ??= "";
            startLocation.State = locations[0].AdminArea3 ??= "";
            startLocation.Country = locations[0].AdminArea1 ??= "";

            tour.StartLocation = startLocation;

            var targetLocation = new Address();
            targetLocation.Street = locations[1].Street ??= "";
            targetLocation.PostalCode = locations[1].PostalCode ??= "";
            targetLocation.City = locations[1].AdminArea5 ??= "";
            targetLocation.County = locations[1].AdminArea4 ??= "";
            targetLocation.State = locations[1].AdminArea3 ??= "";
            targetLocation.Country = locations[1].AdminArea1 ??= "";

            tour.TargetLocation = targetLocation;

        }

    }
}
