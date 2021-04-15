﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlannerApp.Commands;
using TourPlannerApp.ViewModels.Base;

namespace TourPlannerApp.Navigator
{
    public class Navigator : BaseViewModel, INavigator
    {
        private BaseViewModel _currentViewModel;

        public BaseViewModel CurrentViewModel
        {

            get
            {
                return _currentViewModel;
            }

            set
            {
                _currentViewModel = value;
                RaisePropertyChangedEvent(nameof(CurrentViewModel));
            }

        }

        public ICommand UpdateCurrentViewModelCommand => new UpdateCurrentViewModelCommand(this);
    }
}
