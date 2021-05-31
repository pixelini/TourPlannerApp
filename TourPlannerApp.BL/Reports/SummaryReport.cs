using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.Models;

namespace TourPlannerApp.BL.Reports
{
    class SummaryReport : IDocument
    {
        public List<TourItem> Model { get; }

        public SummaryReport(List<TourItem> model)
        {
            Model = model;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IContainer container)
        {
            container
                .PaddingHorizontal(0)
                .PaddingVertical(0)
                .Page(page =>
                {
                    ComposeHeader(page.Header().Height(100).Background("4DB5FF").PaddingHorizontal(50).PaddingTop(40));
                    ComposeContent(page.Content().Background("F1F1F1").PaddingHorizontal(50));
                    page.Footer().Height(50).Background("4DB5FF");
                });
        }

        /*

        public void Compose(IContainer container, List<TourItem> model)
        {
            container
                .PaddingHorizontal(0)
                .PaddingVertical(0)
                .Page(page =>
                {
                    ComposeHeader(page.Header().Height(100).Background("4DB5FF").PaddingHorizontal(50).PaddingTop(40));
                    page.Content().Background("F1F1F1").ComposeContentSpecific(model);
                    page.Footer().Height(50).Background("4DB5FF");
                });

        }
        */

        void ComposeHeader(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeColumn().Stack(stack =>
                {
                    stack.Item().Text($"Statistik: Meine Touren", TextStyle.Default.Size(20).Color("FFF"));
                    stack.Item().Text($"Erstellt am: {DateTime.Now:d}", TextStyle.Default.Color("FFF"));
                });

                row.ConstantColumn(100).Height(50).Placeholder();
            });
        }

        void ComposeContent(IContainer container)
        {

            container.PaddingVertical(40).Stack(stack =>
            {

                stack.Spacing(5);

                stack.Item().Row(row =>
                {
                    //var logEntry = new LogEntryComponent("Gesamtzeit: ", Model.AllTours[0].Log[0]);
                    var logEntry = new LogEntryComponent("Gesamtzeit: ", Model[0].Log[0]);
                    logEntry.Compose(row.RelativeColumn());
                });

                stack.Spacing(20);

                var amountOfLogs = 0;

                foreach (var tour in Model)
                {
                    amountOfLogs += tour.Log.Count();
                }

                stack.Item().AlignLeft().Text($"Anzahl Aktivitäten: {amountOfLogs}", TextStyle.Default.Size(14));

                stack.Item().Element(ComposeTable);

                //var sumOfDistanceOfFirstTour = Model.AllTours[0].Log.Sum(x => x.Distance); // alle distanzen zusammen
                var sumOfDistanceOfFirstTour = Model[0].Log.Sum(x => x.Distance); // alle distanzen zusammen
                
                
            });
        }

        void ComposeTable(IContainer container)
        {
            var textSizeTable = TextStyle.Default.Size(8);

            container.PaddingTop(10).Decoration(decoration =>
            {
                // header
                decoration.Header().BorderBottom(1).Padding(5).Row(row =>
                {
                    row.ConstantColumn(25).Text("#", textSizeTable);
                    row.RelativeColumn().Text("Tourstart", textSizeTable);
                    row.RelativeColumn().Text("Tourende", textSizeTable);
                    row.RelativeColumn().AlignRight().Text("Dauer", textSizeTable);
                    row.RelativeColumn().AlignRight().Text("Distanz", textSizeTable);
                    row.RelativeColumn().AlignRight().Text("Höhenmeter", textSizeTable);
                    row.RelativeColumn().AlignRight().Text("Bewertung", textSizeTable);
                    row.RelativeColumn().AlignRight().Text("Beschreibung", textSizeTable);
                });

                // content
                decoration
                    .Content()
                    .Stack(stack =>
                    {
                        foreach (var tour in Model)
                        {
                            foreach (var item in tour.Log)
                            {
                                stack.Item().BorderBottom(1).BorderColor("CCC").Padding(5).Row(row =>
                                {
                                    row.ConstantColumn(25).Text(Model[0].Log.IndexOf(item) + 1, textSizeTable);
                                    row.RelativeColumn().Text(item.StartTime, textSizeTable);
                                    row.RelativeColumn().Text(item.EndTime, textSizeTable);
                                    row.RelativeColumn().AlignRight().Text(item.OverallTime, textSizeTable);
                                    row.RelativeColumn().AlignRight().Text(item.Distance, textSizeTable);
                                    row.RelativeColumn().AlignRight().Text($"{item.Distance}m", textSizeTable);
                                    row.RelativeColumn().AlignRight().Text(item.Rating, textSizeTable);
                                    row.RelativeColumn().AlignRight().Text(item.Description, textSizeTable);
                                    //row.RelativeColumn().AlignRight().Text($"{item.Price * item.Quantity}$");
                                });
                            }
                        }


                    });
            });
        }

    }

    public static class ReportExtensions
    {
        public static void ComposeContentSpecific(this IContainer container, List<TourItem> model)
        {
            container.PaddingVertical(40).Stack(stack =>
            {
                stack.Spacing(5);

                stack.Item().Row(row =>
                {
                    var logEntry = new LogEntryComponent("LogEntryComponent: ", model[0].Log[0]);
                    logEntry.Compose(row.RelativeColumn());
                });

                var sumOfDistanceOfFirstTour = model[0].Log.Sum(x => x.Distance); // alle distanzen zusammen
                stack.Item().AlignRight().Text($"SumOfDistanceOfFirstTour: {sumOfDistanceOfFirstTour}$", TextStyle.Default.Size(14));
            });
        }
    }
}
