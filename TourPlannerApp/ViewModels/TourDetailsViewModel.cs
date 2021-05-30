using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlannerApp.BL.Services;
using TourPlannerApp.Models;
using TourPlannerApp.ViewModels.Base;
using TourPlannerApp.Views;

namespace TourPlannerApp.ViewModels
{
    public class TourDetailsViewModel : BaseViewModel
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ITourService _tourService { get; set; }

        public AddLogEntryDialog AddLogEntryDialog { get; set; }
        public AddLogEntryDialogViewModel AddLogEntryDialogViewModel { get; set; }

        public TourItem SelectedTour { get; set; }

        public List<LogEntry> Log { get { return SelectedTour.Log; } }

        private ICommand _addLogEntryCommand;
        public ICommand AddLogEntryCommand => _addLogEntryCommand ??= new RelayCommand(AddLogEntry);

        public TourDetailsViewModel(TourItem selectedTour, ITourService tourService)
        {
            SelectedTour = selectedTour;
            Debug.WriteLine("neuew TourDetailsViewModel erstellt");
            Debug.WriteLine("SelectedTour: "+ SelectedTour.Id);
            _tourService = tourService;
            SelectedTour.Log = _tourService.GetAllLogsForTour(SelectedTour);
            Debug.WriteLine("Current Log: " + SelectedTour.Log);
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
            _tourService.AddTourLog(SelectedTour.Id, newLogEntry);
            SelectedTour.Log =_tourService.GetAllLogsForTour(SelectedTour);
            Debug.WriteLine("Current Log: " + SelectedTour.Log);


            //RefreshTourList();

            // TODO: if successfull -> close
            AddLogEntryDialog.Close();
        }

    }
}
