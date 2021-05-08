using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TourPlannerApp.BL.Services;
using TourPlannerApp.DAL;
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

        private string distance;

        public string Distance
        {
            get
            {
                return distance;
            }

            set
            {
                if (distance != value)
                {
                    distance = value;
                    RaisePropertyChangedEvent(nameof(Distance));
                }
            }
        }

        public ObservableCollection<Maneuver> Description { get; set; }


        private Byte[] tourImage;

        public Byte[] TourImage
        {
            get
            {
                return tourImage;
            }

            set
            {
                if (tourImage != value)
                {
                    tourImage = value;
                    RaisePropertyChangedEvent(nameof(TourImage));
                }
            }
        }

        private TourLookupItem currentTour;

        public TourLookupItem CurrentTour
        {
            get
            {
                return currentTour;
            }

            set
            {

                if (currentTour != value)
                {
                    currentTour = value;
                    // Set all other properties
                    SetResultProperties(currentTour);
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                    Status = "Tour gefunden!";
                }
            }
        }

        private void SetResultProperties(TourLookupItem currentTour)
        {

            if (currentTour.Route.Locations != null)
            {
                StartLocationResult = FormatLocation(currentTour.Route.Locations[0]);
                TargetLocationResult = FormatLocation(currentTour.Route.Locations[1]);
            }

            Distance = currentTour.Route.Distance.ToString();
            Description = new ObservableCollection<Maneuver>(GetAllManeuvers(currentTour));
            TourImage = _tourLookupService.GetTourImage(currentTour); // //TODO: success? -> add image couldn't be loaded -> other properties are ok
            RaisePropertyChangedEvent(nameof(Description));

        }

        private ObservableCollection<Maneuver> GetAllManeuvers(TourLookupItem currentTour)
        {
            var allManeuvers = new ObservableCollection<Maneuver>();

            foreach (var maneuver in currentTour.Route.Legs[0].Maneuvers)
            {
                allManeuvers.Add(maneuver);
            }

            return allManeuvers;

        }

        private void LookupTour()
        {
            Debug.WriteLine("Tour Lookup...");
            Debug.WriteLine(StartLocationInput);
            Debug.WriteLine(TargetLocationInput);
            var tourResult = _tourLookupService.GetTour(StartLocationInput, TargetLocationInput);
            CurrentTour = tourResult;
            
        }


        private string FormatLocation(Location location)
        {
            var fullLocation = "";

            if (location.Street != null)
            {
                fullLocation = AddToLocationString(fullLocation, location.Street);
            }

            if (location.PostalCode != null)
            {
                fullLocation = AddToLocationString(fullLocation, location.PostalCode);
            }

            if (location.AdminArea4 != null)
            {
                fullLocation = AddToLocationString(fullLocation, location.AdminArea4);
            }

            if (location.AdminArea3 != null)
            {
                fullLocation = AddToLocationString(fullLocation, location.AdminArea3);
            }

            if (location.AdminArea1 != null)
            {
                fullLocation = AddToLocationString(fullLocation, location.AdminArea1);
            }

            return fullLocation;
        }

        private string AddToLocationString(string locationString, string locationDetail)
        {
            if (locationString == "")
            {
                locationString += locationDetail;
            } else
            {
                locationString += ", " + locationDetail;
            }

            return locationString;
        }



        public AddTourViewModel()
        {
            var repository = new TourDataAccess();
            _tourService = new TourService(repository);
            var api = new TourLookupDataAccess();
            _tourLookupService = new TourLookupService(api);
            TourImage = null;
            CurrentTour = null;
            Status = "Noch keine Suche gestartet.";
        }

    }
    
    
    
}
