using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.Navigator;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public INavigator Navigator { get; set; } = new Navigator.Navigator();

        public MainViewModel()
        {
            Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Home);
        }
    }
}
