using System.Collections.Generic;
using TourPlannerApp.Models;
using static TourPlannerApp.Models.Models.TourLookup;

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
        public int AddTourLog(int id, LogEntry newLogEntry);
        public List<LogEntry> GetAllLogsForTour(TourItem selectedTour);
    }
}