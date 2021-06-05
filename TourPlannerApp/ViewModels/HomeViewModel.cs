using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlannerApp.BL.Services;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;
using TourPlannerApp.Store;
using TourPlannerApp.ViewModels.Base;
using TourPlannerApp.Views;

namespace TourPlannerApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ITourService _tourService { get; set; }

        public EditTourDialog EditTourDialog { get; set; }
        public EditTourDialogViewModel EditTourDialogViewModel { get; set; }

        private TourItem _currentItem;
        public TourItem CurrentItem
        {
            get
            {
                return _currentItem;
            }

            set
            {
                if ((_currentItem != value) && (value != null))
                {
                    _currentItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentItem));
                }
            }

        }

        private ICommand _showSummaryReportCommand;
        public ICommand ShowSummaryReportCommand => _showSummaryReportCommand ??= new RelayCommand(ShowSummaryReport);

        private ICommand _saveSummaryReportCommand;
        public ICommand SaveSummaryReportCommand => _saveSummaryReportCommand ??= new RelayCommand(SaveSummaryReport);

        private ICommand _importTourCommand;
        public ICommand ImportTourCommand => _importTourCommand ??= new RelayCommand(ImportTourData);


        public HomeViewModel()
        {
            var repository = new TourDataAccess();
            var imgFolder = new PictureAccess();
            _tourService = new TourService(repository, imgFolder);
        }

        private void ShowSummaryReport()
        {
            _tourService.ShowSummaryReport();
        }

        private void SaveSummaryReport()
        {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Pdf|*.pdf";
            saveFileDialog.Title = "Speicherort auswählen...";
            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                var filePath = saveFileDialog.FileName;
                _tourService.SaveSummaryReport(filePath);
            }
            Debug.WriteLine(saveFileDialog.FileName);
        }

        private void ImportTourData()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Json|*.json";
            openFileDialog.Title = "Json auswählen...";

            if (openFileDialog.ShowDialog() == true)
            {
                var filePath = openFileDialog.FileName;
                var newTour = _tourService.ImportTourData(filePath);

                CurrentItem = newTour;
                CurrentItem.Id = -1;
                EditTourDialogViewModel = new EditTourDialogViewModel(newTour);
                EditTourDialogViewModel.Save += EditTourDialogViewModelOnSave;
                EditTourDialog = new EditTourDialog(EditTourDialogViewModel);
                bool? isClosed = EditTourDialog.ShowDialog();
                Debug.WriteLine("Is Open: " + isClosed);
                //Navigator.CurrentViewModel = new TourDetailsViewModel(CurrentItem, _tourService);
                
            }

        }

        public void EditTourDialogViewModelOnSave(object sender, EventArgs eventArgs)
        {
            // get new Data
            CurrentItem.Name = EditTourDialogViewModel.TournameInput;
            CurrentItem.Description = EditTourDialogViewModel.DescriptionInput;

            // TODO: Validate it
            _tourService.AddImportedTour(CurrentItem);
            TourEvents.AnnounceNewTour();

            // TODO: if successfull -> close
            EditTourDialog.Close();
        }


    }
}