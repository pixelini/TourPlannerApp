using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlannerApp.Navigator;
using TourPlannerApp.ViewModels;

namespace TourPlannerApp.Commands
{
    public class UpdateCurrentViewModelCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private INavigator _navigator;

        public UpdateCurrentViewModelCommand(INavigator navigator)
        {
            _navigator = navigator;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (parameter is ViewType viewType)
            {
                switch (viewType)
                {
                    case ViewType.Home:
                        _navigator.CurrentViewModel = new HomeViewModel();
                        break;
                    case ViewType.AddTour:
                        _navigator.CurrentViewModel = new AddTourViewModel();
                        break;
                    case ViewType.TourDetails:
                        _navigator.CurrentViewModel = new TourDetailsViewModel();
                        break;
                    default:
                        break;
                }

            }
        }

    }
}
