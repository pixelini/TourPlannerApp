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
using TourPlannerApp.Models;
using TourPlannerApp.ViewModels;

namespace TourPlannerApp.Views
{
    /// <summary>
    /// Interaktionslogik für AddLogEntryDialog.xaml
    /// </summary>
    public partial class EditTourDialog : Window
    {
        /*
        public EditTourDialog(TourItem currentTour)
        {
            InitializeComponent();
            //this.DataContext = new EditTourDialogViewModel(currentTour);
        }
        */

        public EditTourDialog()
        {
            InitializeComponent();
        }

        public EditTourDialog(EditTourDialogViewModel dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
            //dataContext.Save += Save;
        }

        void Save(object sender, EventArgs e)
        {
            //this.Close();
        }
    }
}
