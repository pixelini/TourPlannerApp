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


        public string GetStartLocationAsString()
        {
            var fullLocation = "";

            if (StartLocation.Street != null)
            {
                fullLocation = AddToLocationString(fullLocation, StartLocation.Street);
            }

            if (StartLocation.PostalCode != null)
            {
                fullLocation = AddToLocationString(fullLocation, StartLocation.PostalCode);
            }

            if (StartLocation.City != null)
            {
                fullLocation = AddToLocationString(fullLocation, StartLocation.City);
            }

            /*
            if (address.County != null)
            {
                fullLocation = AddToLocationString(fullLocation, address.County);
            }
            */

            if (StartLocation.State != null)
            {
                fullLocation = AddToLocationString(fullLocation, StartLocation.State);
            }

            if (StartLocation.Country != null)
            {
                fullLocation = AddToLocationString(fullLocation, StartLocation.Country);
            }

            return fullLocation;
        }

        public string GetTargetLocationAsString()
        {
            var fullLocation = "";

            if (TargetLocation.Street != null)
            {
                fullLocation = AddToLocationString(fullLocation, TargetLocation.Street);
            }

            if (TargetLocation.PostalCode != null)
            {
                fullLocation = AddToLocationString(fullLocation, TargetLocation.PostalCode);
            }

            if (TargetLocation.City != null)
            {
                fullLocation = AddToLocationString(fullLocation, TargetLocation.City);
            }

            /*
            if (address.County != null)
            {
                fullLocation = AddToLocationString(fullLocation, address.County);
            }
            */

            if (TargetLocation.State != null)
            {
                fullLocation = AddToLocationString(fullLocation, TargetLocation.State);
            }

            if (TargetLocation.Country != null)
            {
                fullLocation = AddToLocationString(fullLocation, TargetLocation.Country);
            }

            return fullLocation;
        }

        private string AddToLocationString(string locationString, string locationDetail)
        {
            if (locationString == "")
            {
                locationString += locationDetail;
            }
            else
            {
                locationString += ", " + locationDetail;
            }

            return locationString;
        }



    }
}