using System.Windows;
using TourPlannerApp.ViewModels;

namespace TourPlannerApp
{
    public partial class MainWindow : Window
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow()
        {
            _log.Info("Hello Tourlogging!");
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
