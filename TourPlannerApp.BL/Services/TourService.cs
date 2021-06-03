using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TourPlannerApp.BL.Reports;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;

namespace TourPlannerApp.BL.Services
{
    public class TourService : ITourService
    {
        private ITourDataAccess _tourDataAccess;

        private IPictureAccess _tourPictureAccess;

        //public TourService(ITourDataAccess tourDataAccess)
        //{
        //    _tourDataAccess = tourDataAccess;
        //}

        public TourService(ITourDataAccess tourDataAccess, IPictureAccess tourImgAccess)
        {
            _tourDataAccess = tourDataAccess;
            _tourPictureAccess = tourImgAccess;
        }

        public List<TourItem> GetAllTours()
        {
            var allTours = _tourDataAccess.GetAllTours();
            allTours = ValidateImgPaths(allTours);
            return allTours;
        }

        private List<TourItem> ValidateImgPaths(List<TourItem> tourList)
        {
            foreach (var tour in tourList)
            {
                if (!_tourPictureAccess.Exists(tour.PathToImg))
                {
                    tour.PathToImg = "/Images/default.png";
                }
            }

            return tourList;
        }

        public int AddTour(TourItem newTourItem)
        {
            int tourId = -1;

            if (!Exists(newTourItem))
            {
                var pathToImg = "";

                // Save tour img in file system
                if (newTourItem.Image != null)
                {
                    pathToImg = _tourPictureAccess.SavePicture(newTourItem.Image);
                }              

                if (pathToImg != "") // if no error
                {
                    newTourItem.PathToImg = pathToImg;
                } else
                {
                    newTourItem.PathToImg = "-";
                }

                tourId = _tourDataAccess.AddTour(newTourItem);
                if (tourId >= 0)
                {
                    Debug.WriteLine("Tour was successfully added.");
                    return tourId;
                }

                Debug.WriteLine("Tour couldn't be added.");
                return tourId;
            }

            Debug.WriteLine("Tour already exits.");
            
            return tourId;
        }

        public bool UpdateTour(TourItem tourItem)
        {
            if (Exists(tourItem))
            {
                // update
                bool success = _tourDataAccess.UpdateTour(tourItem);
                if (success)
                {
                    Debug.WriteLine("Tour was successfully updated.");
                    return true;
                }

                Debug.WriteLine("Tour couldn't be updated.");
                return false;
            }

            Debug.WriteLine("Tour doesn't exits.");

            return false;
        }

        public bool DeleteTour(TourItem tourItem)
        {
            // find by id
            if (Exists(tourItem))
            {
                Debug.WriteLine("Tour exists.");
                
                // check if log entries exist
                if (_tourDataAccess.DoesTourHaveLogs(tourItem.Id))
                {
                    // delete logs of tour
                    _tourDataAccess.DeleteAllLogEntries(tourItem.Id);
                }

                // delete if tour exists
                if (_tourDataAccess.DeleteTour(tourItem))
                {
                    Debug.WriteLine("Tour was successfully deleted.");

                    // Delete Image
                    _tourPictureAccess.DeletePicture(tourItem.PathToImg);
                    return true;
                }

                Debug.WriteLine("Tour couldn't be deleted.");
                return false;
            }

            Debug.WriteLine("Tour does not exist.");
            return false;
        }

        public List<TourItem> SearchByName(string tourName)
        {
            throw new System.NotImplementedException();
        }

        public bool Exists(TourItem tourItem)
        {
            return _tourDataAccess.Exists(tourItem);
        }

        public bool Exists(int tourId, LogEntry logEntry)
        {
            return _tourDataAccess.Exists(tourId, logEntry);
        }

        public int AddTourLog(int tourId, LogEntry newLogEntry)
        {
            return _tourDataAccess.AddTourLog(tourId, newLogEntry);
        }

        public bool UpdateTourLog(int tourId, LogEntry editedLogEntry)
        {
            return _tourDataAccess.UpdateTourLog(tourId, editedLogEntry);
        }

        public List<LogEntry> GetAllLogsForTour(TourItem selectedTour)
        {
            return _tourDataAccess.GetAllLogsForTour(selectedTour);
        }

        public bool DeleteLogEntry(TourItem selectedTour, LogEntry selectedLogEntry)
        {
            // find by id
            if (Exists(selectedTour.Id, selectedLogEntry))
            {
                Debug.WriteLine("Log exists.");
                // delete if log exists
                if (_tourDataAccess.DeleteLogEntry(selectedTour, selectedLogEntry))
                {
                    Debug.WriteLine("Log was successfully deleted.");
                    return true;
                }

                Debug.WriteLine("Log couldn't be deleted.");
                return false;
            }

            Debug.WriteLine("Log does not exist.");
            return false;
        }

        public void ShowSummaryReport()
        {
            var guid = Guid.NewGuid().ToString().Substring(0, 6);
            var allToursWithLogs = GetAllTours();

            foreach (var tour in allToursWithLogs)
            {
                tour.Log = GetAllLogsForTour(tour);
            }

            var filePath = $"SummaryReport_{(DateTime.Now):dd_MM_yyyy}_{guid}.pdf";
            var document = new SummaryReport("Statistik: Meine Touren", allToursWithLogs);
            document.GeneratePdf(filePath);
            Process.Start("explorer.exe", filePath);
        }

        public void ShowTourReport(TourItem selectedTour)
        {
            var guid = Guid.NewGuid().ToString().Substring(0, 6);
            // get current tour logs
            selectedTour.Log = GetAllLogsForTour(selectedTour);
            
            var filePath = $"TourReport_{selectedTour.Id}_{(DateTime.Now):dd_MM_yyyy}_{guid}.pdf";
            var document = new TourReport($"Report von Tour: {selectedTour.Name}", selectedTour);
            document.GeneratePdf(filePath);
            Process.Start("explorer.exe", filePath);
        }

        public void SaveSummaryReport(string filePath)
        {
            var allToursWithLogs = GetAllTours();

            foreach (var tour in allToursWithLogs)
            {
                tour.Log = GetAllLogsForTour(tour);
            }

            var document = new SummaryReport("Statistik: Meine Touren", allToursWithLogs);
            document.GeneratePdf(filePath);
        }

        public void SaveTourReport(TourItem selectedTour, string filePath)
        {
            selectedTour.Log = GetAllLogsForTour(selectedTour);
            var document = new TourReport($"Report von Tour: {selectedTour.Name}", selectedTour);
            document.GeneratePdf(filePath);
        }



        




    }
}