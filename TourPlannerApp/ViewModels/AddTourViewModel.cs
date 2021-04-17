using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.BL.Services;
using TourPlannerApp.DAL;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.ViewModels
{
    public class AddTourViewModel : BaseViewModel
    {
        private ITourService _tourService { get; set; }

        public AddTourViewModel()
        {
            var repository = new TourDataAccess();
            _tourService = new TourService(repository);
        }
        
        
    }
    
    
    
}
