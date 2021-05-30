using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlannerApp.Models;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class LogEntryDialogViewModel : BaseViewModel
    {

        private LogEntry _logEntry;

        public LogEntry LogEntry 
        { 
            get 
            {
                _logEntry.Rating = SelectedRating.ItemValue;
                _logEntry.StartTime = Convert.ToDateTime(_startTimeInput);
                _logEntry.EndTime = Convert.ToDateTime(_endTimeInput);
                _logEntry.Description = _descriptionInput;
                _logEntry.Distance = _distanceInput;
                _logEntry.OverallTime = TimeSpan.Parse(_overallTimeInput);
                _logEntry.Altitude = _altitudeInput;
                return _logEntry; 
            } 
            
            set { } 
        
        }

        public List<SelectableItem> Ratings { get; set; }

        //private SelectableItem _selectedRating;

        public SelectableItem SelectedRating
        {
            get { return Ratings.FirstOrDefault(s => s.IsSelected); }
        }

        private string _startTimeInput;
        public string StartTimeInput
        {
            get
            {
                return _startTimeInput;
            }

            set
            {
                if ((_startTimeInput != value) && (value != null))
                {
                    _startTimeInput = value;
                    RaisePropertyChangedEvent(nameof(StartTimeInput));
                }
            }

        }

        private string _endTimeInput;
        public string EndTimeInput
        {
            get
            {
                return _endTimeInput;
            }

            set
            {
                if ((_endTimeInput != value) && (value != null))
                {
                    _endTimeInput = value;
                    RaisePropertyChangedEvent(nameof(EndTimeInput));
                }
            }

        }

        private string _descriptionInput;
        public string DescriptionInput
        {
            get
            {
                return _descriptionInput;
            }

            set
            {
                if ((_descriptionInput != value) && (value != null))
                {
                    _descriptionInput = value;
                    RaisePropertyChangedEvent(nameof(DescriptionInput));
                }
            }

        }

        private float _distanceInput;
        public float DistanceInput
        {
            get
            {
                return _distanceInput;
            }

            set
            {
                if ((_distanceInput != value))
                {
                    _distanceInput = value;
                    RaisePropertyChangedEvent(nameof(DistanceInput));
                }
            }
        }

        private string _overallTimeInput;
        public string OverallTimeInput
        {
            get
            {
                return _overallTimeInput;
            }

            set
            {
                if ((_overallTimeInput != value) && (value != null))
                {
                    _overallTimeInput = value;
                    RaisePropertyChangedEvent(nameof(OverallTimeInput));
                }
            }
        }

        private float _altitudeInput;
        public float AltitudeInput
        {
            get
            {
                return _altitudeInput;
            }

            set
            {
                if ((_altitudeInput != value))
                {
                    _altitudeInput = value;
                    RaisePropertyChangedEvent(nameof(AltitudeInput));
                }
            }
        }


        public event EventHandler Save;

        private ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new RelayCommand(x => Save(this, new EventArgs()));

        public LogEntryDialogViewModel() 
        {

            Ratings = CreateRatings();
            Ratings[0].IsSelected = true; // first element is default option
            _logEntry = new LogEntry();
            _startTimeInput = DateTime.Today.ToString();
            _endTimeInput = DateTime.Today.ToString();
            _descriptionInput = "";
            _distanceInput = 0;
            _overallTimeInput = new TimeSpan(2, 14, 18).ToString();
            _altitudeInput = 0;

        }

        public LogEntryDialogViewModel(LogEntry logEntry)
        {
            Ratings = CreateRatings();
            var selectedRating = Ratings.FirstOrDefault(s => s.ItemValue == logEntry.Rating); // LINQ :)
            selectedRating.IsSelected = true;
            _logEntry = logEntry;
            _startTimeInput = logEntry.StartTime.ToString();
            _endTimeInput = logEntry.EndTime.ToString();
            _descriptionInput = logEntry.Description;
            _distanceInput = logEntry.Distance;
            _overallTimeInput = logEntry.OverallTime.ToString();
            _altitudeInput = logEntry.Altitude;

        }

        private List<SelectableItem> CreateRatings()
        {
            Ratings = new List<SelectableItem>
            {
                new SelectableItem { ItemValue = 5, ItemDescription = "Sehr gut"},
                new SelectableItem { ItemValue = 4, ItemDescription = "Gut" },
                new SelectableItem { ItemValue = 3, ItemDescription = "So lala" },
                new SelectableItem { ItemValue = 2, ItemDescription = "Meeh" },
                new SelectableItem { ItemValue = 1, ItemDescription = "Nie wieder!" }
            };

            return Ratings;
        }



    }
}
