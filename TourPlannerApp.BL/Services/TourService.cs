using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
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

        public List<TourItem> SearchFullText(string searchInput)
        {
            var allToursWithLogs = GetAllTours();

            if (searchInput == "")
            {
                return allToursWithLogs;
            }
            
            foreach (var tour in allToursWithLogs)
            {
                tour.Log = GetAllLogsForTour(tour);
            }

            var foundItems = new List<TourItem>();
            
            foreach (var tour in allToursWithLogs)
            {
                if (CheckIfTourItemContainsString(tour, searchInput))
                {
                    foundItems.Add(tour);
                }
            }
            
            return foundItems;
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

        public void ExportTourData(TourItem selectedTour, string filePath)
        {
            Debug.WriteLine(filePath);

            var imageDataAsByteArray = ReadImageFile(selectedTour.PathToImg);
            if (imageDataAsByteArray != null)
            {
                selectedTour.Image = imageDataAsByteArray;
            }

            var tourAsJson = JsonConvert.SerializeObject(selectedTour, Formatting.Indented);
            File.WriteAllText(filePath, tourAsJson);
        }

        public TourItem ImportTourData(string filePath)
        {
            // read file into a string and deserialize JSON to a type
            var newTour = JsonConvert.DeserializeObject<TourItem>(File.ReadAllText(filePath));
            
            // if all data here??? -> validate
            return newTour;

        }

        public int AddImportedTour(TourItem currentItem)
        {
            var tourId = AddTour(currentItem);

            // also add existing logs
            if (currentItem.Log.Count > 1)
            {
                foreach (var logEntry in currentItem.Log)
                {
                    AddTourLog(tourId, logEntry);
                }
            }

            return tourId;

        }


        #region Helper Methods

        private bool CheckIfTourItemContainsString(TourItem tour, string searchInput)
        {
            foreach (var prop in tour.GetType().GetProperties())
            {
                if (prop.GetValue(tour) is TourItem.Address)
                {
                    var address = (TourItem.Address)prop.GetValue(tour);
                    if (CheckIfContainsString(address, searchInput))
                    {
                        return true;
                    }
                }
                
                if (prop.Name == "Id") continue;
                
                if (prop.GetValue(tour, null) != null && prop.GetValue(tour, null).ToString().ToLower().Contains($"{searchInput.ToLower()}"))
                {
                    return true;
                }

                if (prop.Name == "Log" && tour.Log.Count > 0 && tour.Log != null)
                {
                    if (tour.Log.Select(logEntry => CheckIfContainsString(logEntry, searchInput)).Any(isMatch => isMatch))
                    {
                        return true;
                    }
                }

            }
            
            return false;
        }

        private bool CheckIfContainsString(object classObject, string searchInput)
        {
            return classObject.GetType().GetProperties()
                .Where(propLog => propLog.Name != "Id")
                .Any(propLog => propLog.GetValue(classObject, null) != null && propLog.GetValue(classObject, null)
                    .ToString().ToLower().Contains($"{searchInput.ToLower()}"));
        }


        private byte[] ReadImageFile(string imageLocation)
        {
            byte[] imageData = null;
            if (File.Exists(imageLocation))
            {
                var fileInfo = new FileInfo(imageLocation);
                var imageFileLength = fileInfo.Length;
                var fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                var br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
            }
            return imageData;
        }

        #endregion




    }
}