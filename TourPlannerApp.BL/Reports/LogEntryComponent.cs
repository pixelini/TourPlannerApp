using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.Models;

namespace TourPlannerApp.BL.Reports
{
    public class LogEntryComponent : IComponent
    {
        public string Title { get; set; }
        LogEntry LogEntry { get; set; }

        public LogEntryComponent(string title, LogEntry logEntry)
        {
            Title = title;
            LogEntry = logEntry;
        }

        public void Compose(IContainer container)
        {
            container.Stack(stack =>
            {
                stack.Spacing(5);

                stack.Item().BorderBottom(1).PaddingBottom(5).Text(Title);
                stack.Item().Text(LogEntry.Rating);
                stack.Item().Text(LogEntry.Distance);
            });
        }
    }
}
