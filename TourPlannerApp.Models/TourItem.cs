using System;
using System.Collections.Generic;

namespace TourPlannerApp.Models
{
    public class TourItem
    {
        public TourItemType Type { get; set; }
        
        public string Id { get; set; }
        
        // Optional
        public string Name { get; set; }

        public string StartLocation { get; set; }

        public string TargetLocation { get; set; }

        public float Distance { get; set; }

        public string PathToImg { get; set; }

        // Turn by turn navigation
        public List<string> NavigationDetails { get; set; }

        public DateTime CreationTime { get; set; }

        // Optional
        public Dictionary<string, string> Log { get; set; }

    }
}