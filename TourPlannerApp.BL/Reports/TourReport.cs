using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlannerApp.Models;
using static System.Net.Mime.MediaTypeNames;

namespace TourPlannerApp.BL.Reports
{
    public class TourReport : IDocument
    {
        public string Title { get; set; }

        public TourItem Model { get; }

        public string PathToLogo { get; set; }

        public TourReport(string title, TourItem model)
        {
            Title = title;
            Model = model;
            PathToLogo = @"C:\Users\Lisi\source\repos\SS2021\SWE\TourPlannerApp\TourPlannerApp\Images\logo_tourbo_white.png";
        }

        private byte[] GetLogo()
        {
            return ReadImageFile(PathToLogo);
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

        void ComposeHeader(IContainer container)
        {
            var header = new HeaderComponent($"{Title}", GetLogo());
            header.Compose(container);
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Stack(stack =>
            {

                //stack.Spacing(5);

                //stack.Item().Row(row =>
                //{
                //    var logEntry = new LogEntryComponent("Gesamtzeit: ", Model.Log[0]);
                //    logEntry.Compose(row.RelativeColumn());
                //});

                //stack.Spacing(20);
                stack.Item().Element(ComposeImage);
                stack.Item().Element(ComposeList);
                //stack.Item().Element(ComposeTable);

                //var sumOfDistanceOfFirstTour = Model.AllTours[0].Log.Sum(x => x.Distance); // alle distanzen zusammen
                var sumOfDistanceOfFirstTour = Model.Log.Sum(x => x.Distance); // alle distanzen zusammen
                
                
            });
        }

        void ComposeList(IContainer container)
        {
            var textSizeHeadings = TextStyle.Default.Size(12).SemiBold();
            var textSizeValues = TextStyle.Default.Size(10);

            int counter = 0;

            container.Stack(stack =>
            {
                stack.Item().PaddingTop(30).PaddingBottom(30).Text($"Meine Aktivitäten", TextStyle.Default.Size(14).Color("4DB5FF").Bold());

                foreach (var item in Model.Log)
                {
                    counter++;

                    stack.Item().BorderTop(1).PaddingTop(20).PaddingBottom(20).Text($"#Nr. {counter}", TextStyle.Default.Size(14).Bold());

                    //stack.Spacing(1);

                    stack.Item().PaddingBottom(5).Text("Tourstart", textSizeHeadings);
                    stack.Item().PaddingBottom(20).Text(item.StartTime, textSizeValues);

                    stack.Item().PaddingBottom(5).Text("Tourende", textSizeHeadings);
                    stack.Item().PaddingBottom(20).Text(item.EndTime, textSizeValues);

                    stack.Item().PaddingBottom(5).Text("Dauer", textSizeHeadings);
                    stack.Item().PaddingBottom(20).Text(item.OverallTime, textSizeValues);

                    stack.Item().PaddingBottom(5).Text("Distanz", textSizeHeadings);
                    stack.Item().PaddingBottom(20).Text(item.Distance, textSizeValues);

                    stack.Item().PaddingBottom(5).Text("Höhenmeter", textSizeHeadings);
                    stack.Item().PaddingBottom(20).Text(item.Altitude, textSizeValues);

                    stack.Item().PaddingBottom(5).Text("Bewertung", textSizeHeadings);
                    stack.Item().PaddingBottom(20).Text(item.Rating, textSizeValues);

                    stack.Item().PaddingBottom(5).Text("Beschreibung", textSizeHeadings);
                    stack.Item().PaddingBottom(20).Text(item.Description, textSizeValues);
                    
                    stack.Item().BorderBottom(1).PaddingBottom(20);
                }




            });






            //var textSizeTable = TextStyle.Default.Size(8);

            //container.PaddingTop(10).Decoration(decoration =>
            //{
            //    // header
            //    decoration.Header().BorderBottom(1).Padding(5).Row(row =>
            //    {
            //        row.ConstantColumn(25).Text("#", textSizeTable);
            //        row.RelativeColumn().Text("Tourstart", textSizeTable);
            //        row.RelativeColumn().Text("Tourende", textSizeTable);
            //        row.RelativeColumn().AlignRight().Text("Dauer", textSizeTable);
            //        row.RelativeColumn().AlignRight().Text("Distanz", textSizeTable);
            //        row.RelativeColumn().AlignRight().Text("Höhenmeter", textSizeTable);
            //        row.RelativeColumn().AlignRight().Text("Bewertung", textSizeTable);
            //        row.RelativeColumn().AlignRight().Text("Beschreibung", textSizeTable);
            //    });

            //    // content
            //    decoration
            //        .Content()
            //        .Stack(stack =>
            //        {
            //            foreach (var item in Model.Log)
            //            {

            //                stack.Item().BorderBottom(1).BorderColor("CCC").Padding(5).Row(row =>
            //                {
            //                    //row.ConstantColumn(25).Text(item.Log.IndexOf(item) + 1, textSizeTable);
            //                    row.RelativeColumn().Text(item.StartTime, textSizeTable);
            //                    row.RelativeColumn().Text(item.EndTime, textSizeTable);
            //                    row.RelativeColumn().AlignRight().Text(item.OverallTime, textSizeTable);
            //                    row.RelativeColumn().AlignRight().Text(item.Distance, textSizeTable);
            //                    row.RelativeColumn().AlignRight().Text($"{item.Distance}m", textSizeTable);
            //                    row.RelativeColumn().AlignRight().Text(item.Rating, textSizeTable);
            //                    row.RelativeColumn().AlignRight().Text(item.Description, textSizeTable);
            //                    //row.RelativeColumn().AlignRight().Text($"{item.Price * item.Quantity}$");
            //                });

            //            }


            //        });
            //});
        }

        void ComposeTable2(IContainer container)
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
                        foreach (var item in Model.Log)
                        {
                   
                            stack.Item().BorderBottom(1).BorderColor("CCC").Padding(5).Row(row =>
                            {
                                //row.ConstantColumn(25).Text(item.Log.IndexOf(item) + 1, textSizeTable);
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


                    });
            });
        }

        void ComposeImage(IContainer container)
        {
            byte[] imageData = ReadImageFile(Model.PathToImg);

            container.PaddingTop(10).Decoration(decoration =>
            {
                // content
                decoration
                    .Content()
                    .Grid(grid =>
                    {
                        grid.VerticalSpacing(0);
                        grid.HorizontalSpacing(20);
                        //grid.AlignCenter();
                        grid.Columns(10); // 12 by default
                        
                        if (imageData != null)
                        {
                            grid.Item(5).Image(imageData);
                        } else
                        {
                            grid.Item().PaddingBottom(20).Text("Kein Bild verfügbar");
                        }
                        grid.Item(5).PaddingBottom(20).Text(
                            $"Tourname: {Model.Name}\n" +
                            $"Startpunkt: {Model.GetStartLocationAsString()}\n" +
                            $"Zielort: {Model.GetTargetLocationAsString()}\n" +
                            $"Distanz: {Model.Distance:0.00} km\n"
                            );

                        grid.VerticalSpacing(30);
                        grid.Item(10).PaddingBottom(20).Text($"Beschreibung: { Model.Description}\n");
                    });

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
                });

                // content
                decoration
                    .Content()
                    .Stack(stack =>
                    {
                        /*
                        stack.Item().BorderBottom(1).BorderColor("CCC").Padding(5).Row(row =>
                        {
                            //row.ConstantColumn(25).Text(item.Log.IndexOf(item) + 1, textSizeTable);
                            row.RelativeColumn().Text(item.StartTime, textSizeTable);
                            row.RelativeColumn().Text(item.EndTime, textSizeTable);
                            row.RelativeColumn().AlignRight().Text(item.OverallTime, textSizeTable);
                            row.RelativeColumn().AlignRight().Text(item.Distance, textSizeTable);
                            row.RelativeColumn().AlignRight().Text($"{item.Distance}m", textSizeTable);
                            row.RelativeColumn().AlignRight().Text(item.Rating, textSizeTable);
                            row.RelativeColumn().AlignRight().Text(item.Description, textSizeTable);
                            //row.RelativeColumn().AlignRight().Text($"{item.Price * item.Quantity}$");
                        });

                        */
                        foreach (var item in Model.Log)
                        {

                            stack.Item().BorderBottom(1).BorderColor("CCC").Padding(5).Row(row =>
                            {
                                row.ConstantColumn(25).Text("FIRST");
                                row.RelativeColumn().Text("Tourstart", textSizeTable);
                                row.RelativeColumn().Text(item.StartTime, textSizeTable);
                                row.RelativeColumn().Text("Tourende", textSizeTable);
                                row.RelativeColumn().Text(item.EndTime, textSizeTable);
                                row.RelativeColumn().Text("Dauer", textSizeTable);
                                row.RelativeColumn().Text(item.OverallTime, textSizeTable);
                                row.RelativeColumn().Text("Distanz", textSizeTable);
                                row.RelativeColumn().Text(item.Distance, textSizeTable);
                                row.RelativeColumn().Text("Höhenmeter", textSizeTable);
                                row.RelativeColumn().Text(item.Altitude, textSizeTable);
                                row.RelativeColumn().Text("Bewertung", textSizeTable);
                                row.RelativeColumn().Text(item.Rating, textSizeTable);
                                row.RelativeColumn().Text("Beschreibung", textSizeTable);
                                row.RelativeColumn().Text(item.Description, textSizeTable);
                            });

                        }


                    });
            });
            

                
        }

        private byte[] ReadImageFile(string imageLocation)
        {
            byte[] imageData = null;
            if (File.Exists(imageLocation))
            {
                FileInfo fileInfo = new FileInfo(imageLocation);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
            }
            return imageData;
        }
    }



    
}
