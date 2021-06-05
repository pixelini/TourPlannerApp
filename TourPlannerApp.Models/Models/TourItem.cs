using System;
using System.Collections.Generic;
using System.Linq;
using static TourPlannerApp.Models.Models.TourLookup;

namespace TourPlannerApp.Models
{
    public class TourItem
    {
        public TourItemType Type { get; set; }
        
        public int Id { get; set; }
        
        // Optional
        public string Name { get; set; }
        public string Description { get; set; }
        public Address StartLocation { get; set; }
        public Address TargetLocation { get; set; }

        public class Address
        {
            public string Street { get; set; }
            public string PostalCode { get; set; }
            public string City { get; set; }
            public string County { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
        }

        public double Distance { get; set; }

        public string PathToImg { get; set; }
        public byte[] Image { get; set; }

        // Turn by turn navigation
        public List<string> NavigationDetails { get; set; }

        public DateTime CreationTime { get; set; }

        // Optional
        public List<LogEntry> Log { get; set; }
        
        
        public string GetStartLocationAsString() => GetLocationString(StartLocation);

        public string GetTargetLocationAsString() => GetLocationString(TargetLocation);

        private string GetLocationString(Address address)
        {
            var strArray = new List<string> { address.Street, address.PostalCode, address.City, address.County, address.Country };
            return String.Join(", ", strArray.Where(m => !String.IsNullOrEmpty(m)).ToList());
        }
        

    }
}