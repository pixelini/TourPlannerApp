using System;
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


        private void LookupTour()
        {
            Debug.WriteLine("Tour Lookup...");
            Debug.WriteLine(StartLocationInput);
            Debug.WriteLine(TargetLocationInput);
            var tourResult = _tourLookupService.GetTour(StartLocationInput, TargetLocationInput);
            Debug.WriteLine("Distance: " + tourResult.Route.Distance);
            Debug.WriteLine("Von: " + tourResult.Route.Locations[0].AdminArea5);
            Debug.WriteLine("Von Street: " + tourResult.Route.Locations[0].Street);
            Debug.WriteLine("Nach: " + tourResult.Route.Locations[1].AdminArea5);
            Debug.WriteLine("Nach Street: " + tourResult.Route.Locations[1].Street);

            TourImage = _tourLookupService.GetTourImage(tourResult);
        }


        public AddTourViewModel()
        {
            var repository = new TourDataAccess();
            _tourService = new TourService(repository);
            var api = new TourLookupDataAccess();
            _tourLookupService = new TourLookupService(api);
            TourImage = null;
        }

        public BitmapImage ImageFromBuffer(Byte[] bytes)
        {
            MemoryStream stream = new MemoryStream(bytes);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            image.StreamSource = stream;
            image.EndInit();
            return image;
        }

    }
    
    
    
}
