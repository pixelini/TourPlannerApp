using System;
using System.Windows.Input;
using TourPlannerApp.Models;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class EditTourDialogViewModel : BaseViewModel
    {
        
        private readonly log4net.ILog _logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        public TourItem CurrentTour { get; set; }

        public string StartLocationFull { get => CurrentTour.GetStartLocationAsString(); set { } }

        public string TargetLocationFull { get => CurrentTour.GetTargetLocationAsString(); set { } }
        
        private string _tournameInput;
        public string TournameInput
        {
            get
            {
                return _tournameInput;
            }

            set
            {
                if ((_tournameInput != value) && (value != null))
                {
                    _tournameInput = value;
                    RaisePropertyChangedEvent(nameof(TournameInput));
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



        public event EventHandler Save;

        private ICommand _saveCommand;
        public ICommand SaveCommand => _saveCommand ??= new RelayCommand(x => Save(this, new EventArgs()));

        public EditTourDialogViewModel(TourItem currentTour)
        {
            _logger.Debug($"Ctor: Edit Tour: {currentTour.Name}");
            CurrentTour = currentTour;
            _tournameInput = CurrentTour.Name;
            _descriptionInput = CurrentTour.Description;
        }



    }
}
