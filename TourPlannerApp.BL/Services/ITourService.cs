using System.Collections.Generic;
using TourPlannerApp.Models;

namespace TourPlannerApp.BL.Services
{
    public interface ITourService
    {
        public List<TourItem> GetAllTours();

        public int AddTour(TourItem newTourItem);

        public bool UpdateTour(TourItem tourItem);

        public bool DeleteTour(TourItem tourItem);
        
        public List<TourItem> SearchFullText(string searchInput);
        
        public bool Exists(TourItem tourItem);

        public bool Exists(int tourId, LogEntry logEntry);

        public int AddTourLog(int tourId, LogEntry newLogEntry);

        public List<LogEntry> GetAllLogsForTour(TourItem selectedTour);

        public bool DeleteLogEntry(TourItem selectedTour, LogEntry selectedLogEntry);

        public bool UpdateTourLog(int tourId, LogEntry editedLogEntry);
        
        public void ShowSummaryReport();

        public void SaveSummaryReport(string filePath);

        public void ShowTourReport(TourItem selectedTour);

        public void SaveTourReport(TourItem selectedTour, string filePath);

        public void ExportTourData(TourItem selectedTour, string filePath);

        public TourItem ImportTourData(string filePath);
        
        public int AddImportedTour(TourItem currentItem);
    }
}