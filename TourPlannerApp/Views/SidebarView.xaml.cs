using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourPlannerApp.ViewModels;

namespace TourPlannerApp.Views
{
    /// <summary>
    /// Interaktionslogik für SidebarViewModel.xaml
    /// </summary>
    public partial class SidebarView : UserControl
    {
        public SidebarView()
        {
            InitializeComponent();
            this.DataContext = new SidebarViewModel();
        }
    }
}
