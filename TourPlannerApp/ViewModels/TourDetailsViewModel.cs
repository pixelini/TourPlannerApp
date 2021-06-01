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

        public LogEntryDialog LogEntryDialog { get; set; }
        public LogEntryDialogViewModel LogEntryDialogViewModel { get; set; }

        public TourItem SelectedTour { get; set; }

        private ObservableCollection<LogEntry> _logEntryInfos { get; set; }

        public ObservableCollection<LogEntry> LogEntryInfos
        {
            get { return _logEntryInfos; }
            set
            {
                _logEntryInfos = value;
                RaisePropertyChangedEvent(nameof(LogEntryInfos));
            }
        }

        private ICommand _addLogEntryCommand;
        public ICommand AddLogEntryCommand => _addLogEntryCommand ??= new RelayCommand(AddLogEntry);

        private ICommand _editLogEntryCommand;
        public ICommand EditLogEntryCommand => _editLogEntryCommand ??= new RelayCommand(EditLogEntry);

        private ICommand _deleteLogEntryCommand;
        public ICommand DeleteLogEntryCommand => _deleteLogEntryCommand ??= new RelayCommand(DeleteLogEntry);

        public TourDetailsViewModel(TourItem selectedTour, ITourService tourService)
        {
            SelectedTour = selectedTour;
            _tourService = tourService;
            SelectedTour.Log = _tourService.GetAllLogsForTour(SelectedTour);
            LogEntryInfos = new ObservableCollection<LogEntry>(_tourService.GetAllLogsForTour(SelectedTour));
        }
        private void AddLogEntry()
        {
            //var newLogEntry = new LogEntry();
            LogEntryDialogViewModel = new LogEntryDialogViewModel();
            LogEntryDialogViewModel.Save += AddLogEntryDialogViewModelOnSave;
            LogEntryDialog = new LogEntryDialog(LogEntryDialogViewModel);

            LogEntryDialog.ShowDialog();
        }

        public void AddLogEntryDialogViewModelOnSave(object sender, EventArgs eventArgs)
        {
            var newLogEntry = LogEntryDialogViewModel.LogEntry;

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
            LogEntryInfos = new ObservableCollection<LogEntry>(_tourService.GetAllLogsForTour(SelectedTour));

            // TODO: if successfull -> close
            LogEntryDialog.Close();
        }


        private void EditLogEntry(object parameter)
        {
            var selectedLogEntry = (LogEntry)parameter;
            LogEntryDialogViewModel = new LogEntryDialogViewModel(selectedLogEntry);
            LogEntryDialogViewModel.Save += UpdateLogEntryDialogViewModelOnSave;
            LogEntryDialog = new LogEntryDialog(LogEntryDialogViewModel);
            LogEntryDialog.ShowDialog();
        }

        private void UpdateLogEntryDialogViewModelOnSave(object sender, EventArgs e)
        {
            var editedLogEntry = LogEntryDialogViewModel.LogEntry;

            // TODO: Validate it
            if (_tourService.UpdateTourLog(SelectedTour.Id, editedLogEntry))
            {
                SelectedTour.Log = _tourService.GetAllLogsForTour(SelectedTour);
                LogEntryInfos = new ObservableCollection<LogEntry>(_tourService.GetAllLogsForTour(SelectedTour));
            }

            // TODO: if successfull -> close
            LogEntryDialog.Close();
        }


        private void DeleteLogEntry(object parameter)
        {
            var selectedLogEntry = (LogEntry)parameter;

            // Delete Entry from log
            if (_tourService.DeleteLogEntry(SelectedTour, selectedLogEntry))
            {
                LogEntryInfos.Remove(selectedLogEntry);
            } else
            {
                Debug.WriteLine("TourLog konnte nicht geloescht werden.");
            }

        }
    }





    
}
