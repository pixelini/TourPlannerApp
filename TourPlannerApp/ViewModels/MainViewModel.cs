using TourPlannerApp.Navigator;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static INavigator Navigator { get; set; } = new Navigator.Navigator();

        public MainViewModel()
        {
            Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Home);
        }
    }
}
