using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.Models;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class TourDetailsViewModel : BaseViewModel
    {
        public TourItem SelectedTour { get; set; }

        public TourDetailsViewModel()
        {
            //SelectedTour = selectedTour;
        }

    }
}
