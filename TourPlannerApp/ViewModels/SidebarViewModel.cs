using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.BL.Services;
using TourPlannerApp.DAL;
using TourPlannerApp.Models;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class SidebarViewModel : BaseViewModel
    {
        private ITourService _tourService { get; set; }


        public ObservableCollection<TourItem> Items { get; set; }


        public SidebarViewModel()
        {
            var repository = new TourDataAccess();
            _tourService = new TourService(repository);
            this.Items = new ObservableCollection<TourItem>(_tourService.GetAllTours());
        }
    }
}
