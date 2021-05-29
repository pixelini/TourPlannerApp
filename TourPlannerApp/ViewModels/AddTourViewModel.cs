﻿using System.Diagnostics;
using System.Windows.Input;
using TourPlannerApp.BL.Services;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;
using TourPlannerApp.Store;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class AddTourViewModel : BaseViewModel
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private ITourService _tourService { get; set; }

        private ITourLookupService _tourLookupService { get; set; }

        public string StartLocationInput { get; set; }

        public string TargetLocationInput { get; set; }

        private ICommand lookupTourCommand;
        public ICommand LookupTourCommand => lookupTourCommand ??= new RelayCommand(LookupTour);


        // tour results
        private bool _showResult;
        public bool ShowResult
        {
            get
            {
                return _showResult;
            }

            set
            {
                if (_showResult != value)
                {
                    _showResult = value;
                    RaisePropertyChangedEvent(nameof(ShowResult));
                }
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return _status;
            }

            set
            {
                if (_status != value)
                {
                    _status = value;
                    RaisePropertyChangedEvent(nameof(Status));
                }
            }
        }

        private string _startLocationResult;
        public string StartLocationResult
        {
            get
            {
                return _startLocationResult;
            }

            set
            {
                if (_startLocationResult != value)
                {
                    _startLocationResult = value;
                    RaisePropertyChangedEvent(nameof(StartLocationResult));
                }
            }
        }

        private string _targetLocationResult;
        public string TargetLocationResult
        {
            get
            {
                return _targetLocationResult;
            }

            set
            {
                if (_targetLocationResult != value)
                {
                    _targetLocationResult = value;
                    RaisePropertyChangedEvent(nameof(TargetLocationResult));
                }
            }
        }

        private string _tourNameInput;
        public string TourNameInput
        {
            get
            {
                return _tourNameInput;
            }

            set
            {
                if (_tourNameInput != value)
                {
                    _tourNameInput = value;
                    RaisePropertyChangedEvent(nameof(TourNameInput));
                }
            }
        }

        private TourItem _currentTourLookup;
        public TourItem CurrentTourLookup
        {
            get
            {
                return _currentTourLookup;
            }

            set
            {

                if (_currentTourLookup != value)
                {
                    _currentTourLookup = value;
                    SetFormattedProperties();
                    RaisePropertyChangedEvent(nameof(CurrentTourLookup));
                    Status = "Tour gefunden!";
                }
            }
        }

        private ICommand _saveTourLookupCommand;
        public ICommand SaveTourLookupCommand => _saveTourLookupCommand ??= new RelayCommand(SaveTour);

        public AddTourViewModel()
        {
            _log.Info("Add Tour...");
            var repository = new TourDataAccess();
            _tourService = new TourService(repository);
            var api = new TourLookupDataAccess();
            _tourLookupService = new TourLookupService(api);
            Status = "Noch keine Suche gestartet.";
            ShowResult = false;
        }


        private void SetFormattedProperties()
        {
            StartLocationResult = FormatLocationData(CurrentTourLookup.StartLocation);
            TargetLocationResult = FormatLocationData(CurrentTourLookup.TargetLocation);
            TourNameInput = "Meine Tour 1";
        }


        private void LookupTour()
        {
            var tourResult = _tourLookupService.GetTour(StartLocationInput, TargetLocationInput);
            if (tourResult == null)
            {
                Status = "Keine Tour gefunden.";
                ShowResult = false;
                return;
            }
            ShowResult = true;
            CurrentTourLookup = tourResult;
        }


        private void SaveTour(object commandParameter)
        {
            Debug.WriteLine("Add Tour...");
            CurrentTourLookup.Name = TourNameInput;
            _tourService.AddTour(CurrentTourLookup);
            TourEvents.AnnounceNewTour();

        }

        #region Helpers

        private string FormatLocationData(TourItem.Address address)
        {
            var fullLocation = "";

            if (address.Street != null)
            {
                fullLocation = AddToLocationString(fullLocation, address.Street);
            }

            if (address.PostalCode != null)
            {
                fullLocation = AddToLocationString(fullLocation, address.PostalCode);
            }

            if (address.County != null)
            {
                fullLocation = AddToLocationString(fullLocation, address.County);
            }

            if (address.Country != null)
            {
                fullLocation = AddToLocationString(fullLocation, address.Country);
            }

            return fullLocation;
        }

        private string AddToLocationString(string locationString, string locationDetail)
        {
            if (locationString == "")
            {
                locationString += locationDetail;
            }
            else
            {
                locationString += ", " + locationDetail;
            }

            return locationString;
        }


        #endregion



    }



}
