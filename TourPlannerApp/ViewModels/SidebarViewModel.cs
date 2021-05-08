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

namespace TourPlannerApp.ViewModels
{
    public class SidebarViewModel : BaseViewModel
    {
        public INavigator Navigator { get; set; }

        private ITourService _tourService { get; set; }

        public ObservableCollection<TourItem> Items { get; set; }

        public ObservableCollection<TourItem> SearchResultItems { get; set; }


        private string searchInput;
        private TourItem currentItem;

        private ICommand searchCommand;
        private ICommand deleteCommand;
        public ICommand SearchCommand => searchCommand ??= new RelayCommand(Search);
        public ICommand DeleteCommand => deleteCommand ??= new RelayCommand(Delete);

        public TourItem CurrentItem
        {
            get
            {
                return currentItem;
            }

            set
            {
                if ((currentItem != value) && (value != null))
                {
                    currentItem = value;
                    RaisePropertyChangedEvent(nameof(CurrentItem));
                }
            }

        }

        public string SearchInput
        {
            get
            {
                return searchInput;
            }

            set
            {
                if (searchInput != value)
                {
                    searchInput = value;
                    
                    RaisePropertyChangedEvent(nameof(SearchInput));
                    Search(SearchInput);
                    
                }
            }
        }


        public SidebarViewModel()
        {
            // sets navigator to the static navigator of the MainViewModel
            Navigator = MainViewModel.Navigator;
            var repository = new TourDataAccess();
            _tourService = new TourService(repository);
            Items = new ObservableCollection<TourItem>(_tourService.GetAllTours());
            SearchResultItems = new ObservableCollection<TourItem>(Items);
            SearchInput = "";
            currentItem = new TourItem();
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
