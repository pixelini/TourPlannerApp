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
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ITourService _tourService { get; set; }

        private ICommand _showSummaryReportCommand;
        public ICommand ShowSummaryReportCommand => _showSummaryReportCommand ??= new RelayCommand(ShowSummaryReport);

        private ICommand _saveSummaryReportCommand;
        public ICommand SaveSummaryReportCommand => _saveSummaryReportCommand ??= new RelayCommand(SaveSummaryReport);

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
            saveFileDialog.Title = "Speicherort auswählen ...";
            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                var filePath = saveFileDialog.FileName;
                _tourService.SaveSummaryReport(filePath);
            }
            Debug.WriteLine(saveFileDialog.FileName);
        }
    }
}
