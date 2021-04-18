using System;
using System.Collections.Generic;
using System.Windows.Controls;
using TourPlannerApp.Models;

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

            // Todo --> back to data access
            List<Log> logs = new List<Log>();
            logs.Add(new Log() { StartTime = DateTime.Today, EndTime = DateTime.Today,  Description = "Es war furchtbar. Immer nur Regen :(", Distance = (float)130.9,  OverallTime = new DateTime(1971, 7, 23), Rating = 5 });
            logs.Add(new Log() { StartTime = DateTime.Today, EndTime = DateTime.Today, Description = "Es war toll. Immer nur Regen :)", Distance = (float)130.9, OverallTime = new DateTime(1971, 7, 23), Rating = 5 });
            logs.Add(new Log() { StartTime = DateTime.Today, EndTime = DateTime.Today, Description = "Baaaaah... nie wieder!", Distance = (float)130.9, OverallTime = new DateTime(1971, 7, 23), Rating = 5 });
            dgLogs.ItemsSource = logs;
        }
    }

}
