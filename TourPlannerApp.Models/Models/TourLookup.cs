using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            public BoundingBox boundingBox { get; set; }
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
            public Lr lr { get; set; }
            public Ul ul { get; set; }
        }

        public class Lr
        {
            public double lng { get; set; }
            public double lat { get; set; }
        }

        public class Ul
        {
            public double lng { get; set; }
            public double lat { get; set; }
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


    }
}
