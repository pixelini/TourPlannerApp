using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using TourPlannerApp.BL.Services;
using TourPlannerApp.Models;
using TourPlannerApp.ViewModels.Base;
using TourPlannerApp.Views;

namespace TourPlannerApp.ViewModels
{
    public class TourDetailsViewModel : BaseViewModel
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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

        private ICommand _showTourReportCommand;
        public ICommand ShowTourReportCommand => _showTourReportCommand ??= new RelayCommand(ShowTourReport);

        private ICommand _saveTourReportCommand;
        public ICommand SaveTourReportCommand => _saveTourReportCommand ??= new RelayCommand(SaveTourReport);

        private ICommand _exportTourCommand;
        public ICommand ExportTourCommand => _exportTourCommand ??= new RelayCommand(ExportTourData);

        

        public TourDetailsViewModel(TourItem selectedTour, ITourService tourService)
        {
            _logger.Debug($"Ctor: TourDetails from tour (ID: {selectedTour.Id}, Name: {selectedTour.Name}...");
            SelectedTour = selectedTour;
            _tourService = tourService;
            SelectedTour.Log = _tourService.GetAllLogsForTour(SelectedTour);
            LogEntryInfos = new ObservableCollection<LogEntry>(_tourService.GetAllLogsForTour(SelectedTour));
        }
        private void AddLogEntry()
        {
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
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                _logger.Debug($"{property.Name} = {property.GetValue(newLogEntry, null)}");
            }

            _tourService.AddTourLog(SelectedTour.Id, newLogEntry);
            SelectedTour.Log =_tourService.GetAllLogsForTour(SelectedTour);
            LogEntryInfos = new ObservableCollection<LogEntry>(_tourService.GetAllLogsForTour(SelectedTour));
            
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

            if (_tourService.UpdateTourLog(SelectedTour.Id, editedLogEntry))
            {
                SelectedTour.Log = _tourService.GetAllLogsForTour(SelectedTour);
                LogEntryInfos = new ObservableCollection<LogEntry>(_tourService.GetAllLogsForTour(SelectedTour));
            }

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
                _logger.Debug("Tourlog couln't be deleted.");
            }

        }

        private void ShowTourReport()
        {
            _tourService.ShowTourReport(SelectedTour);
        }

        private void SaveTourReport()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pdf|*.pdf";
            saveFileDialog.Title = "Speicherort auswählen ...";
            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                var filePath = saveFileDialog.FileName;
                _tourService.SaveTourReport(SelectedTour, filePath);
            }
            Debug.WriteLine(saveFileDialog.FileName);
        }

        
        private void ExportTourData()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json|*.json";
            saveFileDialog.Title = "Speicherort auswählen ...";
            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                var filePath = saveFileDialog.FileName;
                _tourService.ExportTourData(SelectedTour, filePath);
            }
        }
    }





    
}
