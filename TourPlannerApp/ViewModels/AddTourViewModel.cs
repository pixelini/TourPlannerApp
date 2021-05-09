using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TourPlannerApp.BL.Services;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;
using TourPlannerApp.Models.Models;
using TourPlannerApp.ViewModels.Base;
using static TourPlannerApp.Models.Models.TourLookup;

namespace TourPlannerApp.ViewModels
{
    public class AddTourViewModel : BaseViewModel
    {
        private ITourService _tourService { get; set; }

        private ITourLookupService _tourLookupService { get; set; }

        public string StartLocationInput { get; set; }

        public string TargetLocationInput { get; set; }

        private ICommand lookupTourCommand;
        public ICommand LookupTourCommand => lookupTourCommand ??= new RelayCommand(LookupTour);


        // necessary for tour results
        private string status;

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                if (status != value)
                {
                    status = value;
                    RaisePropertyChangedEvent(nameof(Status));
                }
            }
        }

        private string startLocationResult;

        public string StartLocationResult
        {
            get
            {
                return startLocationResult;
            }

            set
            {
                if (startLocationResult != value)
                {
                    startLocationResult = value;
                    RaisePropertyChangedEvent(nameof(StartLocationResult));
                }
            }
        }

        private string targetLocationResult;

        public string TargetLocationResult
        {
            get
            {
                return targetLocationResult;
            }

            set
            {
                if (targetLocationResult != value)
                {
                    targetLocationResult = value;
                    RaisePropertyChangedEvent(nameof(TargetLocationResult));
                }
            }
        }

        private string tourNameInput;

        public string TourNameInput
        {
            get
            {
                return tourNameInput;
            }

            set
            {
                if (tourNameInput != value)
                {
                    tourNameInput = value;
                    RaisePropertyChangedEvent(nameof(TourNameInput));
                }
            }
        }

        private TourItem currentTourLookup;

        public TourItem CurrentTourLookup
        {
            get
            {
                return currentTourLookup;
            }

            set
            {

                if (currentTourLookup != value)
                {
                    currentTourLookup = value;
                    SetFormattedProperties();
                    RaisePropertyChangedEvent(nameof(CurrentTourLookup));
                    Status = "Tour gefunden!";
                }
            }
        }

        private ICommand saveTourLookupCommand;
        public ICommand SaveTourLookupCommand => saveTourLookupCommand ??= new RelayCommand(SaveTour);

        public AddTourViewModel()
        {
            var repository = new TourDataAccess();
            _tourService = new TourService(repository);
            var api = new TourLookupDataAccess();
            _tourLookupService = new TourLookupService(api);
            Status = "Noch keine Suche gestartet.";
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
            CurrentTourLookup = tourResult;
        }


        private void SaveTour(object commandParameter)
        {
            Debug.WriteLine("Add Tour...");
            //_tourService.AddTour(CurrentTourLookup, TourNameInput, StartLocationInput, TargetLocationInput);
            //RefreshTourList();
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
