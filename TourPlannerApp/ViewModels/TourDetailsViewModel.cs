using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlannerApp.Models;
using TourPlannerApp.ViewModels.Base;
using TourPlannerApp.Views;

namespace TourPlannerApp.ViewModels
{
    public class TourDetailsViewModel : BaseViewModel
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TourItem SelectedTour { get; set; }

        private ICommand _addLogEntryCommand;
        public ICommand AddLogEntryCommand => _addLogEntryCommand ??= new RelayCommand(AddLogEntry);

        public TourDetailsViewModel(TourItem selectedTour)
        {
            SelectedTour = selectedTour;
        }
        private void AddLogEntry()
        {
            var window = new AddLogEntryDialog();
            window.ShowDialog();
            //throw new NotImplementedException();
        }

    }
}
