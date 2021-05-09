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

namespace TourPlannerApp.ViewModels
{
    public class SidebarViewModel : BaseViewModel
    {
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

        private ICommand _searchCommand;
        private ICommand _deleteCommand;
        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(Search);
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(Delete);


        public SidebarViewModel()
        {
            // sets navigator to the static navigator of the MainViewModel
            Navigator = MainViewModel.Navigator;
            var repository = new TourDataAccess();
            _tourService = new TourService(repository);
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
        
        
        private void Delete(object commandParameter)
        {
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
