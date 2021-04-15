using System.Windows;
using TourPlannerApp.ViewModels;

namespace TourPlannerApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
