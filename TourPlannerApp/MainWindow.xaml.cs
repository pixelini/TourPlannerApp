using System.Windows;
using TourPlannerApp.ViewModels;

namespace TourPlannerApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            App.Log.Info("Hello Tourlogging!");
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
