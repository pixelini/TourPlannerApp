using System;
using System.Collections.Generic;
using static TourPlannerApp.Models.Models.TourLookup;

namespace TourPlannerApp.Models
{
    public class TourItem
    {
        public TourItemType Type { get; set; }
        
        public int Id { get; set; }
        
        // Optional
        public string Name { get; set; }

        public Address StartLocation { get; set; } // weg
        public Address TargetLocation { get; set; }

        public class Address
        {
            public string Street { get; set; }
            public string PostalCode { get; set; }
            public string County { get; set; }
            public string Country { get; set; }
        }

        //public string StartLocationStreet { get; set; }
        //public string StartLocationPostalCode { get; set; }
        //public string StartLocationCounty { get; set; }
        //public string StartLocationCountry { get; set; }

        //public string TargetLocation { get; set; } // weg

        public string TargetLocationStreet { get; set; }
        public string TargetLocationPostalCode { get; set; }
        public string TargetLocationCounty { get; set; }
        public string TargetLocationCountry { get; set; }

        public double Distance { get; set; }

        public string PathToImg { get; set; }
        public byte[] Image { get; set; }

        // Turn by turn navigation
        public List<string> NavigationDetails { get; set; }

        public DateTime CreationTime { get; set; }

        // Optional
        public Dictionary<string, string> Log { get; set; }


    }
}