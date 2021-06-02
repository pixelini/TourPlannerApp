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
        
        List<TourItem> SearchByName(string tourName);
        
        public bool Exists(TourItem tourItem);

        public bool Exists(int tourId, LogEntry logEntry);

        public int AddTourLog(int tourId, LogEntry newLogEntry);

        public List<LogEntry> GetAllLogsForTour(TourItem selectedTour);

        public bool DeleteLogEntry(TourItem selectedTour, LogEntry selectedLogEntry);

        public bool UpdateTourLog(int tourId, LogEntry editedLogEntry);
        
        public void ShowSummaryReport();
    }
}