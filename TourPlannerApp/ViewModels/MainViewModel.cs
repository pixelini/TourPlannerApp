using TourPlannerApp.Navigator;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static INavigator Navigator { get; set; } = new Navigator.Navigator();

        public MainViewModel()
        {
            _logger.Debug($"Ctor: Main View...");
            Navigator.UpdateCurrentViewModelCommand.Execute(ViewType.Home);
        }
    }
}
