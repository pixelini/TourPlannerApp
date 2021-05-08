using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.ViewModels.Base;


// tour model for the overview tourlist
namespace TourPlannerApp.ViewModels
{
    public class TourListViewModel : BaseViewModel
    {
        public ObservableCollection<TourListItemViewModel> RealItems { get; set; }

        public TourListViewModel()
        {
            RealItems = new ObservableCollection<TourListItemViewModel>
            {
                new TourListItemViewModel
                {
                    Tourname = "Elisabeth",
                    DistanceInfo = "20"
                },

                new TourListItemViewModel
                {
                    Tourname = "Philipp",
                    DistanceInfo = "20"
                },

                new TourListItemViewModel
                {
                    Tourname = "Sepp",
                    DistanceInfo = "20"
                }

            };
        }

    }
}
