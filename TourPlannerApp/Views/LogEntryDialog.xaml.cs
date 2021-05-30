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
using System.Windows.Shapes;
using TourPlannerApp.ViewModels;

namespace TourPlannerApp.Views
{
    /// <summary>
    /// Interaktionslogik für AddLogEntryDialog.xaml
    /// </summary>
    public partial class LogEntryDialog : Window
    {
        public LogEntryDialog(LogEntryDialogViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }
    }
}
