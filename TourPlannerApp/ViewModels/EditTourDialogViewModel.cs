using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlannerApp.Models;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class EditTourDialogViewModel : BaseViewModel
    {

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

        public RelayCommand SaveCommand { get; set; }

        public EditTourDialogViewModel(TourItem currentTour)
        {
            CurrentTour = currentTour;
            _tournameInput = CurrentTour.Name;
            _descriptionInput = CurrentTour.Description;

            SaveCommand = new RelayCommand(x => this.Save(this, new EventArgs()));
        }


        /*

        private bool _closeDialog;
        public bool CloseDialog
        {
            get {
                return _closeDialog; 
            }

            set
            {
                if (_closeDialog != value)
                {
                    _closeDialog = value;
                    RaisePropertyChangedEvent(nameof(CloseDialog));
                }
            }

        }

        private string _tourname;
        public string Tourname
        {
            get
            {
                return _tourname;
            }

            set
            {
                if ((_tourname != value) && (value != null))
                {
                    _tourname = value;
                    RaisePropertyChangedEvent(nameof(Tourname));
                }
            }

        }

        private string _startLocation;
        public string StartLocation
        {
            get
            {
                return _startLocation;
            }

            set
            {
                if ((_startLocation != value) && (value != null))
                {
                    _startLocation = value;
                    RaisePropertyChangedEvent(nameof(StartLocation));
                }
            }

        }

        private string _targetLocation;
        public string TargetLocation
        {
            get
            {
                return _targetLocation;
            }

            set
            {
                if ((_targetLocation != value) && (value != null))
                {
                    _targetLocation = value;
                    RaisePropertyChangedEvent(nameof(TargetLocation));
                }
            }

        }

        private double _distance;
        public double Distance
        {
            get
            {
                return _distance;
            }

            set
            {
                if ((_distance != value) && (value != null))
                {
                    _distance = value;
                    RaisePropertyChangedEvent(nameof(Distance));
                }
            }

        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                if ((_description != value) && (value != null))
                {
                    _description = value;
                    RaisePropertyChangedEvent(nameof(Description));
                }
            }

        }

        private ICommand _updateTourCommand;
        public ICommand UpdateTourCommand => _updateTourCommand ??= new RelayCommand(UpdateTour);

        public EditTourDialogViewModel(TourItem currentTour)
        {
            _tourname = currentTour.Name;
            _startLocation = currentTour.GetStartLocationAsString();
            _targetLocation = currentTour.GetTargetLocationAsString();
            _distance = currentTour.Distance;
            _description = "Blablabla...";
        }

        private void UpdateTour()
        {
            TourItem item = null;

            // if successful
            CloseDialog = true;
            return;
            
            //throw new NotImplementedException();
        }

        */


    }
}
