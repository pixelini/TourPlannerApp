using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TourPlannerApp.ViewModels.Base
{
    public class SelectableItem : BaseViewModel
    {
        public int ItemValue { get; set; }
        public string ItemDescription { get; set; }

        public bool IsSelected { get; set; }

    }
}
