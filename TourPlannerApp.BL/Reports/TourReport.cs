using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.IO;
using TourPlannerApp.Models;

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
                stack.Item().Element(ComposeTourInfo);
                stack.Item().Element(ComposeList);
            });
        }

        void ComposeList(IContainer container)
        {
            var textSizeHeadings = TextStyle.Default.Size(12).SemiBold();
            var textSizeValues = TextStyle.Default.Size(10);
            var counter = 0;

            container.Stack(stack =>
            {
                stack.Item().PaddingTop(30).PaddingBottom(30).Text($"Meine Aktivitäten", TextStyle.Default.Size(14).Color("4DB5FF").Bold());

                if (Model.Log.Count == 0)
                {
                    stack.Item().PaddingBottom(5).Text("Du hast noch keine Aktivitäten für diese Tour aufgezeichnet.", textSizeHeadings);
                }
                else
                {
                    foreach (var entry in Model.Log)
                    {
                        counter++;

                        stack.Item().BorderTop(1).PaddingTop(20).PaddingBottom(20).Text($"#Nr. {counter}", TextStyle.Default.Size(14).Bold());
                    
                        stack.Item().PaddingBottom(5).Text("Tourstart", textSizeHeadings);
                        stack.Item().PaddingBottom(20).Text(entry.StartTime, textSizeValues);

                        stack.Item().PaddingBottom(5).Text("Tourende", textSizeHeadings);
                        stack.Item().PaddingBottom(20).Text(entry.EndTime, textSizeValues);

                        stack.Item().PaddingBottom(5).Text("Dauer", textSizeHeadings);
                        stack.Item().PaddingBottom(20).Text(entry.OverallTime, textSizeValues);

                        stack.Item().PaddingBottom(5).Text("Distanz", textSizeHeadings);
                        stack.Item().PaddingBottom(20).Text(entry.Distance, textSizeValues);

                        stack.Item().PaddingBottom(5).Text("Höhenmeter", textSizeHeadings);
                        stack.Item().PaddingBottom(20).Text(entry.Altitude, textSizeValues);

                        stack.Item().PaddingBottom(5).Text("Bewertung", textSizeHeadings);
                        stack.Item().PaddingBottom(20).Text(entry.Rating, textSizeValues);

                        stack.Item().PaddingBottom(5).Text("Wetter", textSizeHeadings);
                        stack.Item().PaddingBottom(20).Text(entry.Weather, textSizeValues);
                        
                        stack.Item().PaddingBottom(5).Text("Durchschn. Geschwindigkeit in km/h", textSizeHeadings);
                        stack.Item().PaddingBottom(20).Text(entry.AvgSpeed, textSizeValues);
                        
                        stack.Item().PaddingBottom(5).Text("Teilnehmeranzahl", textSizeHeadings);
                        stack.Item().PaddingBottom(20).Text(entry.NumberOfParticipants, textSizeValues);
                        
                        stack.Item().PaddingBottom(5).Text("Anzahl Pausen", textSizeHeadings);
                        stack.Item().PaddingBottom(20).Text(entry.NumberOfBreaks, textSizeValues);

                        stack.Item().PaddingBottom(5).Text("Beschreibung", textSizeHeadings);
                        stack.Item().PaddingBottom(20).Text(entry.Description, textSizeValues);

                        stack.Item().BorderBottom(1).PaddingBottom(20);
                    }
                }

            });
        }
        
        void ComposeTourInfo(IContainer container)
        {
            byte[] imageData = ReadImageFile(Model.PathToImg);

            container.PaddingTop(10).Decoration(decoration =>
            {
                decoration
                    .Content()
                    .Grid(grid =>
                    {
                        grid.VerticalSpacing(0);
                        grid.HorizontalSpacing(20);
                        grid.Columns(10);
                        
                        if (imageData != null)
                        {
                            grid.Item(5).Image(imageData);
                        } else
                        {
                            grid.Item().PaddingBottom(0).AlignCenter().Text(" ----------- Kein Bild ----------- ");
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
        
        private byte[] ReadImageFile(string imageLocation)
        {
            byte[] imageData = null;
            if (File.Exists(imageLocation))
            {
                var fileInfo = new FileInfo(imageLocation);
                var imageFileLength = fileInfo.Length;
                var fs = new FileStream(imageLocation, FileMode.Open, FileAccess.Read);
                var br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);
            }
            return imageData;
        }
    }

}
