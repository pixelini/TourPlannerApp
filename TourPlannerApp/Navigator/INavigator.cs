using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.Navigator
{
    public enum ViewType
    {
        Home,
        AddTour,
        TourDetails
    }

    public interface INavigator
    {
        BaseViewModel CurrentViewModel { get; set; }
        ICommand UpdateCurrentViewModelCommand { get; }
    }
}
