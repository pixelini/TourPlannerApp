using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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

        public AddLogEntryDialog AddLogEntryDialog { get; set; }
        public AddLogEntryDialogViewModel AddLogEntryDialogViewModel { get; set; }

        public TourItem SelectedTour { get; set; }

        private ICommand _addLogEntryCommand;
        public ICommand AddLogEntryCommand => _addLogEntryCommand ??= new RelayCommand(AddLogEntry);

        public TourDetailsViewModel(TourItem selectedTour)
        {
            SelectedTour = selectedTour;
        }
        private void AddLogEntry()
        {
            //var newLogEntry = new LogEntry();
            AddLogEntryDialogViewModel = new AddLogEntryDialogViewModel();
            AddLogEntryDialogViewModel.Save += AddLogEntryDialogViewModelOnSave;
            AddLogEntryDialog = new AddLogEntryDialog(AddLogEntryDialogViewModel);

            AddLogEntryDialog.ShowDialog();
            //throw new NotImplementedException();
        }

        public void AddLogEntryDialogViewModelOnSave(object sender, EventArgs eventArgs)
        {
            //SelectedTour.Log.Add(AddLogEntryDialogViewModel.LogEntry);
            Debug.WriteLine("Added LogEntry: " + AddLogEntryDialogViewModel.LogEntry);

            var newLogEntry = AddLogEntryDialogViewModel.LogEntry;

            // Get all class properties with reflection
            var type = typeof(LogEntry);
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                Debug.WriteLine("{0} = {1}", property.Name, property.GetValue(newLogEntry, null));
            }

            // TODO: Validate it
            //_tourService.AddTourLog(CurrentItem.Id, newLogEntry);
            //RefreshTourList();

            // TODO: if successfull -> close
            AddLogEntryDialog.Close();
        }

    }
}
