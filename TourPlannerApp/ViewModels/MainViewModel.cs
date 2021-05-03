using TourPlannerApp.Navigator;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public static INavigator Navigator { get; set; } = new Navigator.Navigator();

        public MainViewModel()
        {
            Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Home);
        }
    }
}
