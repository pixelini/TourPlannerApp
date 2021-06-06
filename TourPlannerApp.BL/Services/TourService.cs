using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TourPlannerApp.BL.Reports;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;
using TourPlannerApp.BL.Exceptions;
using TourPlannerApp.DAL.Exceptions;

namespace TourPlannerApp.BL.Services
{
    public class TourService : ITourService
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
            try
            {
                var allTours = _tourDataAccess.GetAllTours();
                allTours = ValidateImgPaths(allTours);
                return allTours;
            }
            catch (DataBaseException e)
            {
                Debug.WriteLine(e);
                _logger.Error($"Database Error: {e}");
            }

            return null;
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
                }
                else
                {
                    newTourItem.PathToImg = "-";
                }

                try
                {
                    tourId = _tourDataAccess.AddTour(newTourItem);
                    if (tourId >= 0)
                    {
                        Debug.WriteLine("Tour was successfully added.");
                        _logger.Info($"Tour successfully added: (ID: {newTourItem.Id}, Name: {newTourItem.Name}).");
                        return tourId;
                    }

                    Debug.WriteLine("Tour couldn't be added.");
                    _logger.Info($"Tour couldn't be added. (ID: {newTourItem.Id}, Name: {newTourItem.Name}).");
                    return tourId;
                }
                catch (DataBaseException e)
                {
                    Debug.WriteLine(e);
                    Debug.WriteLine("Tour couldn't be added. DatabaseException");
                    _logger.Error($"Tour couldn't be added. Database Error: {e}");
                }


            }
            else
            {
                Debug.WriteLine("Tour already exits.");
                _logger.Debug($"Tour already exits. TourCollisionException is thrown.");
                throw new TourCollisionException("Tour already exits.", newTourItem.Name);
            }
 

            return tourId;
        }

        public bool UpdateTour(TourItem tourItem)
        {

            try
            {
                // update
                bool success = _tourDataAccess.UpdateTour(tourItem);
                if (success)
                {
                    Debug.WriteLine("Tour was successfully updated.");
                    _logger.Info($"Tour successfully updated: (ID: {tourItem.Id}, Name: {tourItem.Name}).");
                    return true;
                }

                return false;
            }
            catch (DataBaseException e)
            {
                Debug.WriteLine(e);
                Debug.WriteLine("Tour couldn't be updated.");
                _logger.Error($"Tour couldn't be updated. Database Error: {e}");

            }


            return false;
        }

        public bool DeleteTour(TourItem tourItem)
        {
            // find by id
            if (Exists(tourItem))
            {

                try
                {

                    // check if log entries exist
                    if (_tourDataAccess.DoesTourHaveLogs(tourItem.Id))
                    {
                        // delete logs of tour
                        _tourDataAccess.DeleteAllLogEntries(tourItem.Id);
                    }

                    // delete if tour exists
                    if (_tourDataAccess.DeleteTour(tourItem))
                    {
                        _logger.Info($"Tour successfully deleted: (ID: {tourItem.Id}, Name: {tourItem.Name}).");
                        Debug.WriteLine("Tour was successfully deleted.");

                        // Delete Image
                        _tourPictureAccess.DeletePicture(tourItem.PathToImg);
                        return true;
                    }

                    return false;
                }
                catch (DataBaseException e)
                {
                    Debug.WriteLine(e);
                    Debug.WriteLine("Tour couldn't be deleted. DataBaseException.");
                    _logger.Error($"Tour couldn't be deleted. Database Error: {e}");

                }
                catch (Exception e)
                {
                    Debug.WriteLine("Tour couldn't be deleted. " + e);
                }


            }
            else
            {
                Debug.WriteLine("Tour doesn't exits.");
                _logger.Debug($"Tour doesn't exits. TourNotFoundException is thrown.");
                throw new TourNotFoundException("Tour doesn't exits.", tourItem.Name);
            }

            return false;
        }

        public List<TourItem> SearchFullText(string searchInput)
        {
            try
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
            catch (Exception e)
            {
                _logger.Error($"Unknown Error: {e}");
                Debug.WriteLine(e);
            }

            return null;
        }

        public bool Exists(TourItem tourItem)
        {
            try
            {
                return _tourDataAccess.Exists(tourItem);
            }
            catch (DataBaseException e)
            {
                _logger.Error($"Database Error: {e}");
                Debug.WriteLine(e);
            }

            return false;
        }

        public bool Exists(int tourId, LogEntry logEntry)
        {
            try
            {
                return _tourDataAccess.Exists(tourId, logEntry);
            }
            catch (DataBaseException e)
            {
                _logger.Error($"Database Error: {e}");
                Debug.WriteLine(e);
            }

            return false;
        }

        public int AddTourLog(int tourId, LogEntry newLogEntry)
        {
            try
            {
                return _tourDataAccess.AddTourLog(tourId, newLogEntry);
            }
            catch (DataBaseException e)
            {
                _logger.Error($"Database Error: {e}");
                Debug.WriteLine(e);
            }

            return -1;
        }

        public bool UpdateTourLog(int tourId, LogEntry editedLogEntry)
        {
            try
            {
                return _tourDataAccess.UpdateTourLog(tourId, editedLogEntry);
            }
            catch (DataBaseException e)
            {
                _logger.Error($"Database Error: {e}");
                Debug.WriteLine(e);
            }

            return false;
        }

        public List<LogEntry> GetAllLogsForTour(TourItem selectedTour)
        {
            try
            {
                return _tourDataAccess.GetAllLogsForTour(selectedTour);
            }
            catch (DataBaseException e)
            {
                _logger.Error($"Database Error: {e}");
                Debug.WriteLine(e);
            }

            return null;

        }

        public bool DeleteLogEntry(TourItem selectedTour, LogEntry selectedLogEntry)
        {

            // find by id
            if (Exists(selectedTour.Id, selectedLogEntry))
            {

                try
                {
                    // delete if log exists
                    if (_tourDataAccess.DeleteLogEntry(selectedTour, selectedLogEntry))
                    {
                        Debug.WriteLine("Log was successfully deleted.");
                        _logger.Info($"Log successfully deleted: (ID: {selectedLogEntry.Id}, from Tour: {selectedTour.Name}).");
                        return true;
                    }

                    Debug.WriteLine("Log couldn't be deleted.");
                    _logger.Info($"Log couldn't be deleted. (ID: {selectedLogEntry.Id}, from Tour: {selectedTour.Name}).");

                    return false;
                }
                catch (DataBaseException e)
                {
                    _logger.Error($"Database Error: {e}");
                    Debug.WriteLine(e);
                }

            } else
            {
                Debug.WriteLine("Log does not exist.");
                _logger.Debug($"Log doesn't exits. LogNotFoundException is thrown.");
                throw new LogNotFoundException("Log doesn't exits.", selectedTour.Name);
            }

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

            var filePath = $"SummaryReport_{DateTime.Now:dd_MM_yyyy}_{guid}.pdf";
            var document = new SummaryReport("Statistik: Meine Touren", allToursWithLogs);
            document.GeneratePdf(filePath);
            Process.Start("explorer.exe", filePath);
        }

        public void ShowTourReport(TourItem selectedTour)
        {
            var guid = Guid.NewGuid().ToString().Substring(0, 6);
            // get current tour logs
            selectedTour.Log = GetAllLogsForTour(selectedTour);

            var filePath = $"TourReport_{selectedTour.Id}_{DateTime.Now:dd_MM_yyyy}_{guid}.pdf";
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

            try
            {
                File.WriteAllText(filePath, tourAsJson);
            }
            catch (DirectoryNotFoundException e)
            {
                _logger.Error($"Export Tour Data: Directory not found: {e}");

            } catch (UnauthorizedAccessException e)
            {
                _logger.Error($"Export Tour Data: No access to directory: {e}");

            }

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

