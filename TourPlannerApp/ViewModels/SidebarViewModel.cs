using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlannerApp.BL.Services;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;
using TourPlannerApp.ViewModels.Base;
using System.Diagnostics; //for debug-mode
using TourPlannerApp.Navigator;
using TourPlannerApp.Store;
using TourPlannerApp.Views;
using System.Threading;

namespace TourPlannerApp.ViewModels
{
    public class SidebarViewModel : BaseViewModel
    {
        private readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public EditTourDialog EditTourDialog { get; set; }
        public EditTourDialogViewModel EditTourDialogViewModel { get; set; }
        public INavigator Navigator { get; set; }

        private ITourService _tourService { get; set; }

        public ObservableCollection<TourItem> Items { get; set; }
        public ObservableCollection<TourItem> SearchResultItems { get; set; }

        private string _searchInput;
        public string SearchInput
        {
            get
            {
                return _searchInput;
            }

            set
            {
                if (_searchInput != value)
                {
                    _searchInput = value;

                    RaisePropertyChangedEvent(nameof(SearchInput));
                    Search(SearchInput);

                }
            }
        }

        private TourItem _currentItem;
        public TourItem CurrentItem
        {
            get
            {
                return _currentItem;
            }

            set
            {
                if ((_currentItem != value) && (value != null))
                {
                    _currentItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentItem));
                }
            }

        }

        #region Commands

        private ICommand _searchCommand;
        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(Search);

        private ICommand _showSelectedTourCommand;
        public ICommand ShowSelectedTourCommand => _showSelectedTourCommand ??= new RelayCommand(ShowTour);

        private ICommand _editSelectedTourCommand;
        public ICommand EditSelectedTourCommand => _editSelectedTourCommand ??= new RelayCommand(EditTour);

        private ICommand _deleteSelectedTourCommand;
        public ICommand DeleteSelectedTourCommand => _deleteSelectedTourCommand ??= new RelayCommand(DeleteTour);

        #endregion

        public SidebarViewModel()
        {
            // sets navigator to the static navigator of the MainViewModel
            Navigator = MainViewModel.Navigator;
            var repository = new TourDataAccess();
            var imgFolder = new PictureAccess();
            _tourService = new TourService(repository, imgFolder);
            Items = new ObservableCollection<TourItem>(_tourService.GetAllTours());
            SearchResultItems = new ObservableCollection<TourItem>(Items);
            SearchInput = "";
            _currentItem = new TourItem();

            TourEvents.TourCreated += OnTourCreated;

        }

        private void OnTourCreated()
        {
            RefreshTourList();
        }

        private void Search(object commandParameter)
        {
            SearchResultItems?.Clear();
            
            var foundItems = Items.Where(x => x.Name.ToLower().Contains(SearchInput.ToLower()));

            if (!foundItems.Any())
            {
                Console.WriteLine("No items found.");
            }
            else
            {
                foreach (var foundItem in foundItems)
                {
                    SearchResultItems.Add(foundItem);
                }
            }
        }

        private void ShowTour(object commandParameter)
        {
            //Debug.WriteLine("TESTSTETS: " + commandParameter);
            Debug.WriteLine("Show Tour: " + CurrentItem.Name + "(ID: " + CurrentItem.Id + ")");
            Navigator.CurrentViewModel = new TourDetailsViewModel(CurrentItem, _tourService);
        }

        private void EditTour()
        {
            Debug.WriteLine("Edit Tour: " + CurrentItem.Name + "(ID: " + CurrentItem.Id + ")");
            Navigator.CurrentViewModel = new HomeViewModel();             

            EditTourDialogViewModel = new EditTourDialogViewModel(CurrentItem);
            EditTourDialogViewModel.Save += EditTourDialogViewModelOnSave;
            EditTourDialog = new EditTourDialog(EditTourDialogViewModel);
            bool? isClosed = EditTourDialog.ShowDialog();
            Debug.WriteLine("Is Open: " + isClosed);
            Navigator.CurrentViewModel = new TourDetailsViewModel(CurrentItem, _tourService);

        }

        public void EditTourDialogViewModelOnSave(object sender, EventArgs eventArgs)
        {
            // get new Data
            CurrentItem.Name = EditTourDialogViewModel.TournameInput;
            CurrentItem.Description = EditTourDialogViewModel.DescriptionInput;
            Debug.WriteLine("Edited Tourname: " + CurrentItem.Name);

            // TODO: Validate it
            _tourService.UpdateTour(CurrentItem);
            RefreshTourList();

            // TODO: if successfull -> close
            EditTourDialog.Close();
        }

        private void DeleteTour(object commandParameter)
        {
            // if user want's to delete tour that he has currently open in details, nothing happens
            if (Navigator.CurrentViewModel is TourDetailsViewModel)
            {
                if (((TourDetailsViewModel)Navigator.CurrentViewModel).SelectedTour.Id == CurrentItem.Id)
                {
                    Navigator.CurrentViewModel = new HomeViewModel();
                    return;
                }
                
            }
            Debug.WriteLine("Delete Tour: " + CurrentItem.Name + "(ID: " + CurrentItem.Id + ")");
            _tourService.DeleteTour(CurrentItem);
            RefreshTourList();
        }

        private void RefreshTourList()
        {
            SearchInput = "";
            Items = new ObservableCollection<TourItem>(_tourService.GetAllTours());
            SearchResultItems = new ObservableCollection<TourItem>(Items);
            RaisePropertyChangedEvent(nameof(SearchResultItems));
        }


    }
}
