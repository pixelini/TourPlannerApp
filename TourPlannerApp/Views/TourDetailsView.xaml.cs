using System;
using System.Collections.Generic;
using System.Windows.Controls;
using TourPlannerApp.Models;
using TourPlannerApp.ViewModels;

namespace TourPlannerApp.Views
{
    /// <summary>
    /// Interaktionslogik für TourDetailsViewModel.xaml
    /// </summary>
    public partial class TourDetailsView : UserControl
    {
        public TourDetailsView()
        {
            InitializeComponent();
            //this.DataContext = new TourDetailsViewModel();


            //// Todo --> back to data access
            //List<LogEntry> logs = new List<LogEntry>();
            //logs.Add(new LogEntry() { StartTime = DateTime.Today, EndTime = DateTime.Today,  Description = "Es war furchtbar. Immer nur Regen :(", Distance = (float)130.9,  OverallTime = new TimeSpan(2, 14, 18), Rating = 5 });
            //logs.Add(new LogEntry() { StartTime = DateTime.Today, EndTime = DateTime.Today, Description = "Es war toll. Immer nur Regen :)", Distance = (float)130.9, OverallTime = new TimeSpan(2, 14, 18), Rating = 5 });
            //logs.Add(new LogEntry() { StartTime = DateTime.Today, EndTime = DateTime.Today, Description = "Baaaaah... nie wieder!", Distance = (float)130.9, OverallTime = new TimeSpan(2, 14, 18), Rating = 5 });
            //dgLogs.ItemsSource = logs;
        }
    }

}
