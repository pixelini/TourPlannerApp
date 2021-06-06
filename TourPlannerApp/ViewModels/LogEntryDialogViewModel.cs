using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using TourPlannerApp.Models;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class LogEntryDialogViewModel : BaseViewModel, IDataErrorInfo
    {
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();

        private LogEntry _logEntry;

        public LogEntry LogEntry 
        { 
            get 
            {
                _logEntry.Rating = SelectedRating.ItemValue;
                _logEntry.Weather = SelectedWeatherCondition.ItemValue;
                _logEntry.StartTime = Convert.ToDateTime(_startTimeInput);
                _logEntry.EndTime = Convert.ToDateTime(_endTimeInput);
                _logEntry.Description = _descriptionInput;
                _logEntry.Distance = float.Parse(_distanceInput);
                _logEntry.OverallTime = TimeSpan.Parse(_overallTimeInput);
                _logEntry.Altitude = float.Parse(_altitudeInput);
                _logEntry.AvgSpeed = float.Parse(_avgSpeedInput);
                _logEntry.NumberOfBreaks = Int32.Parse(_numberOfBreaksInput);
                _logEntry.NumberOfParticipants = Int32.Parse(_numberOfParticipantsInput);
                return _logEntry; 
            } 
            
            set { } 
        
        }

        public List<SelectableItem> Ratings { get; set; }
        
        public List<SelectableItem> WeatherConditions { get; set; }

        //private SelectableItem _selectedRating;

        public SelectableItem SelectedRating
        {
            get { return Ratings.FirstOrDefault(s => s.IsSelected); }
        }
        
        public SelectableItem SelectedWeatherCondition
        {
            get { return WeatherConditions.FirstOrDefault(s => s.IsSelected); }
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

        private string _distanceInput;
        public string DistanceInput
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

        private string _altitudeInput;
        public string AltitudeInput
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
        
        
        private string _numberOfBreaksInput;
        public string NumberOfBreaksInput
        {
            get
            {
                return _numberOfBreaksInput;
            }

            set
            {
                if ((_numberOfBreaksInput != value))
                {
                    _numberOfBreaksInput = value;
                    RaisePropertyChangedEvent(nameof(NumberOfBreaksInput));
                }
            }
        }
        
        private string _numberOfParticipantsInput;
        public string NumberOfParticipantsInput
        {
            get
            {
                return _numberOfParticipantsInput;
            }

            set
            {
                if ((_numberOfParticipantsInput != value))
                {
                    _numberOfParticipantsInput = value;
                    RaisePropertyChangedEvent(nameof(NumberOfParticipantsInput));
                }
            }
        }
        
        private string _avgSpeedInput;
        public string AvgSpeedInput
        {
            get
            {
                return _avgSpeedInput;
            }

            set
            {
                if ((_avgSpeedInput != value))
                {
                    _avgSpeedInput = value;
                    RaisePropertyChangedEvent(nameof(AvgSpeedInput));
                }
            }
        }
        
        
        
        public int Weather { get; set; }
        

        public event EventHandler Save;

        private ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new RelayCommand(x => Save(this, new EventArgs()));

        public string Error { get { return null; } }

        public string this[string name]
        {
            get
            {
                string result = null;
                switch (name)
                {
                    case "DescriptionInput":
                        if (String.IsNullOrEmpty(DistanceInput))
                        {
                            break;
                        }
                        
                        if (DescriptionInput.Length > 255)
                        {
                            result = "Beschreibung darf nicht mehr als 255 Zeichen haben.";
                        }
                        break;

                    case "StartTimeInput":
                        if (!String.IsNullOrEmpty(StartTimeInput))
                        {
                            DateTime startDateValue;
                            DateTime endDateValue;
                            bool isStartDateValid;
                            bool isEndDateValid;
                            isStartDateValid = DateTime.TryParse(StartTimeInput, out startDateValue);
                            isEndDateValid = DateTime.TryParse(StartTimeInput, out endDateValue);

                            if (!isStartDateValid)
                            {
                                result = "Ungültiges Datumsformat.";
                            } else if (isStartDateValid && isEndDateValid)
                            {
                                var startDate = DateTime.Parse(StartTimeInput);
                                var endDate = DateTime.Parse(EndTimeInput);
                                if (startDate > endDate)
                                {
                                    result = "Startdatum muss vor Enddatum liegen.";
                                }
                            }
                        }
                        else
                        {
                            result = "Feld darf nicht leer sein.";
                        }
                        
                        break;

                    case "EndTimeInput":
                        if (!String.IsNullOrEmpty(EndTimeInput))
                        {
                            DateTime startDateValue2;
                            DateTime endDateValue2;
                            bool isStartDateValid2;
                            bool isEndDateValid2;
                            isStartDateValid2 = DateTime.TryParse(StartTimeInput, out startDateValue2);
                            isEndDateValid2 = DateTime.TryParse(StartTimeInput, out endDateValue2);

                            if (!isEndDateValid2)
                            {
                                result = "Ungültiges Datumsformat.";
                            } else if (isEndDateValid2 && isStartDateValid2)
                            {
                                var startDate = DateTime.Parse(StartTimeInput);
                                var endDate = DateTime.Parse(EndTimeInput);
                                if (startDate > endDate)
                                {
                                    result = "Startdatum muss vor Enddatum liegen.";
                                }
                            }
                        }
                        else
                        {
                            result = "Feld darf nicht leer sein.";
                        }
                        
                        break;

                    case "DistanceInput":
                        
                        if (!String.IsNullOrEmpty(DistanceInput))
                        {
                            float numericValue;
                            bool isNumber = float.TryParse(DistanceInput, out numericValue);

                            if (!isNumber)
                            {
                                result = "Bitte Zahl eingeben.";
                            } else if (numericValue <= 0)
                            {
                                result = "Distanz muss größer als 0 sein.";
                            }
                        } else
                        {
                            result = "Feld darf nicht leer sein.";
                        }

                        break;

                    case "AltitudeInput": 

                        if (!String.IsNullOrEmpty(AltitudeInput))
                        {
                            float numericValue;
                            bool isNumber = float.TryParse(AltitudeInput, out numericValue);

                            if (!isNumber)
                            {
                                result = "Bitte Zahl eingeben.";
                            }
                        } else
                        {
                            result = "Feld darf nicht leer sein.";
                        }

                        break;

                    case "AvgSpeedInput":

                        if (!String.IsNullOrEmpty(AvgSpeedInput))
                        {
                            float numericValue;
                            bool isNumber = float.TryParse(AvgSpeedInput, out numericValue);

                            if (!isNumber)
                            {
                                result = "Bitte Zahl eingeben.";
                            }
                        } else
                        {
                            result = "Feld darf nicht leer sein.";
                        }

                        break;

                    case "NumberOfBreaksInput":

                        if (!String.IsNullOrEmpty(NumberOfBreaksInput))
                        {
                            int numericValue;
                            bool isNumber = Int32.TryParse(NumberOfBreaksInput, out numericValue);

                            if (!isNumber)
                            {
                                result = "Bitte ganze Zahl eingeben.";
                            }
                        } else
                        {
                            result = "Feld darf nicht leer sein.";
                        }

                        break;

                    case "NumberOfParticipantsInput":

                        if (!String.IsNullOrEmpty(NumberOfParticipantsInput))
                        {
                            int numericValue;
                            bool isNumber = Int32.TryParse(NumberOfParticipantsInput, out numericValue);

                            if (!isNumber)
                            {
                                result = "Bitte ganze Zahl eingeben.";
                            }
                        } else
                        {
                            result = "Feld darf nicht leer sein.";
                        }

                        break;


                    case "OverallTimeInput":

                        if (!String.IsNullOrEmpty(OverallTimeInput))
                        {
                            TimeSpan timeSpan;
                            bool isTimeSpan = TimeSpan.TryParse(OverallTimeInput, out timeSpan);

                            if (!isTimeSpan)
                            {
                                result = "Bitte im Format hh:mm:ss eingeben.";
                            }
                        } else
                        {
                            result = "Feld darf nicht leer sein.";
                        }

                        break;

                }

                if (ErrorCollection.ContainsKey(name))
                {
                    ErrorCollection[name] = result;
                } else if (result != null) {
                    ErrorCollection.Add(name, result);
                }

                RaisePropertyChangedEvent(nameof(ErrorCollection));

                return result;
            }
        }


        public LogEntryDialogViewModel() 
        {
            _logger.Debug($"Ctor: New Log Entry...");
            Ratings = CreateRatings();
            WeatherConditions = CreateWeatherConditions();
            Ratings[0].IsSelected = true; // first element is default option
            WeatherConditions[0].IsSelected = true; // first element is default option
            _logEntry = new LogEntry();
            _startTimeInput = DateTime.Today.ToString();
            _endTimeInput = DateTime.Today.AddDays(1).ToString();
            _descriptionInput = "Toll!";
            _distanceInput = 1.ToString();
            _overallTimeInput = new TimeSpan(2, 14, 18).ToString();
            _altitudeInput = 0.ToString();
            _avgSpeedInput = 0.ToString();
            _numberOfBreaksInput = 2.ToString();
            _numberOfParticipantsInput = 1.ToString();
        }

        public LogEntryDialogViewModel(LogEntry logEntry)
        {
            Ratings = CreateRatings();
            WeatherConditions = CreateWeatherConditions();
            var selectedRating = Ratings.FirstOrDefault(s => s.ItemValue == logEntry.Rating); // LINQ :)
            selectedRating.IsSelected = true;
            
            var selectedWeather = WeatherConditions.FirstOrDefault(s => s.ItemValue == logEntry.Weather);
            selectedWeather.IsSelected = true;
            
            _logEntry = logEntry;
            _startTimeInput = logEntry.StartTime.ToString();
            _endTimeInput = logEntry.EndTime.ToString();
            _descriptionInput = logEntry.Description;
            _distanceInput = logEntry.Distance.ToString();
            _overallTimeInput = logEntry.OverallTime.ToString();
            _altitudeInput = logEntry.Altitude.ToString();
            _avgSpeedInput = logEntry.AvgSpeed.ToString();
            _numberOfBreaksInput = logEntry.NumberOfBreaks.ToString();
            _numberOfParticipantsInput = logEntry.NumberOfParticipants.ToString();
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
        
        private List<SelectableItem> CreateWeatherConditions()
        {
            WeatherConditions = new List<SelectableItem>
            {
                new SelectableItem { ItemValue = 1, ItemDescription = "Sonnig"},
                new SelectableItem { ItemValue = 2, ItemDescription = "Bewoelkt" },
                new SelectableItem { ItemValue = 3, ItemDescription = "Regen" }
            };

            return WeatherConditions;
        }



    }
}
