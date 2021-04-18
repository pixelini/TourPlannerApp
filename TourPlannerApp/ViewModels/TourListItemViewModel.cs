using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class TourListItemViewModel : BaseViewModel
    {
        public string Tourname { get; set; }

        public string RouteInfo { get; set; }

        public string DistanceInfo { get; set; }
    }
}
