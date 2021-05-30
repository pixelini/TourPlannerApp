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
            Debug.WriteLine("neuew TourDetailsViewModel erstellt");
            Debug.WriteLine("SelectedTour: "+ SelectedTour.Id);
            _tourService = tourService;
            SelectedTour.Log = _tourService.GetAllLogsForTour(SelectedTour);
            Debug.WriteLine("Current Log: " + SelectedTour.Log);

            LogEntryInfos = new ObservableCollection<LogEntry>(_tourService.GetAllLogsForTour(SelectedTour));

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
            LogEntryInfos = new ObservableCollection<LogEntry>(_tourService.GetAllLogsForTour(SelectedTour));
            Debug.WriteLine("Current Log: " + SelectedTour.Log);


            //RefreshTourList();

            // TODO: if successfull -> close
            AddLogEntryDialog.Close();
        }


        private void EditLogEntry(object parameter)
        {
            var selectedLogEntry = (LogEntry)parameter;
            AddLogEntryDialogViewModel = new AddLogEntryDialogViewModel(selectedLogEntry);
            AddLogEntryDialogViewModel.Save += UpdateLogEntryDialogViewModelOnSave;
            AddLogEntryDialog = new AddLogEntryDialog(AddLogEntryDialogViewModel);
            AddLogEntryDialog.ShowDialog();

        }

        private void UpdateLogEntryDialogViewModelOnSave(object sender, EventArgs e)
        {
            Debug.WriteLine("Update LogEntry: " + AddLogEntryDialogViewModel.LogEntry);

            var editedLogEntry = AddLogEntryDialogViewModel.LogEntry;

            // TODO: Validate it
            if (_tourService.UpdateTourLog(SelectedTour.Id, editedLogEntry))
            {
                SelectedTour.Log = _tourService.GetAllLogsForTour(SelectedTour);
                LogEntryInfos = new ObservableCollection<LogEntry>(_tourService.GetAllLogsForTour(SelectedTour));
            }

            // TODO: if successfull -> close
            AddLogEntryDialog.Close();
        }


        private void DeleteLogEntry(object parameter)
        {
            var selectedLogEntry = (LogEntry)parameter;
            Debug.WriteLine("Delete LogEntry with ID: " + selectedLogEntry.Id);

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
