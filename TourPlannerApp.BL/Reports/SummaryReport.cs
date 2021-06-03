using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TourPlannerApp.Models;

namespace TourPlannerApp.BL.Reports
{
    public class SummaryReport : IDocument
    {
        public string Title { get; set; }

        public List<TourItem> Model { get; }

        public string PathToLogo { get; set; }

        public SummaryReport(string title, List<TourItem> model)
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
                    //ComposeStatisticsList(page.Content().Background("F1F1F1").PaddingHorizontal(50));
                    ComposeContent(page.Content().Background("F1F1F1").PaddingHorizontal(50));
                    page.Footer().Height(50).Background("4DB5FF");
                });
        }

        void ComposeHeader(IContainer container)
        {
            var header = new HeaderComponent(Title, GetLogo());
            header.Compose(container);
        }

        void ComposeContent(IContainer container)
        {
            container.PaddingVertical(40).Stack(stack =>
            {
                stack.Spacing(5);
                stack.Spacing(20);
                stack.Item().Element(ComposeStatisticsList);
            });
        }

        void ComposeStatisticsList(IContainer container)
        {
            // Calculate statistical values
            
            // Anzahl aller Aktivitäten
            var countActivities = GetCountActivities();
            
            // Aktivitätszeit Gesamt
            var sumActivityTimeRounded = (int)GetSumActivityTimeInHours();
            
            // Distanz Gesamt
            var sumActivityDistance = GetSumActivityDistance();
            
            // Höhenmeter Gesamt
            var sumActivityAltitude = GetSumActivityAltitude();
            
            // Längste Tour
            var longestActivityData = GetActivityWithLongestDistance();

            // Durschnittliche Bewertung aller Aktivitäten
            var avgRating = GetAvgRatingOfActivity();

            // Wie viele Touren mit Rating 5 bewertet?
            var countActivitiesWithHighestRating = GetCountActivitiesWithHighestRating();
            
            // Wie war das Wetter? Regen/Sonne: ( Regenrate? )
            float rainRate;
            float sunRate;

            container.Stack(stack =>
            {
                var textSizeHeadings = TextStyle.Default.Size(12).SemiBold();
                var textSizeValues = TextStyle.Default.Size(10);

                if (countActivities == 0)
                {
                    stack.Item().PaddingBottom(5).Text("Du hast noch keine Aktivitäten aufgezeichnet.", textSizeHeadings);
                }
                else
                {
                    stack.Item().PaddingBottom(5).Text("Anzahl aller Aktivitäten:", textSizeHeadings);
                    stack.Item().PaddingBottom(20).Text($"{countActivities}", textSizeValues);

                    stack.Item().PaddingBottom(5).Text("Aktivitätszeit insgesamt:", textSizeHeadings);
                    stack.Item().PaddingBottom(20).Text($"{sumActivityTimeRounded} h", textSizeValues);

                    stack.Item().PaddingBottom(5).Text("Distanz insgesamt:", textSizeHeadings);
                    stack.Item().PaddingBottom(20).Text($"{sumActivityDistance} km", textSizeValues);

                    stack.Item().PaddingBottom(5).Text("Höhenmeter insgesamt:", textSizeHeadings);
                    stack.Item().PaddingBottom(20).Text($"{sumActivityAltitude}", textSizeValues);
                
                    stack.Item().PaddingBottom(5).Text("Durschnittliche Bewertung deiner Aktivitäten:", textSizeHeadings);
                    stack.Item().PaddingBottom(20).Text($"{avgRating}", textSizeValues);
                
                    stack.Item().PaddingBottom(20).Text($"Du hast {countActivitiesWithHighestRating} von insgesamt {GetCountActivities()} Touren mit der Bestnote bewertet.", textSizeHeadings);

                    stack.Item().PaddingBottom(20).Text($"Deine längste Tourdistanz war: {longestActivityData.Distance} km. " + 
                                                        $"(Tour \"{longestActivityData.TourName}\" am {longestActivityData.Date.ToString("dd.MM.yyyy")})", textSizeHeadings);
                }

                stack.Item().BorderBottom(1).PaddingBottom(20);

            });
            
        }

        private int GetCountActivitiesWithHighestRating()
        {
            var countActivities = 0;
            foreach (var tour in Model)
            {
                var result = tour.Log.Where(x => x.Rating == 5);
                countActivities += result.Count();
            }
            return countActivities;
        }

        private float GetAvgRatingOfActivity()
        {
            var sumAllRatings = Model.Sum(tour => tour.Log.Sum(x => x.Rating));
            return sumAllRatings / (float)GetCountActivities();
        }

        private float GetSumActivityAltitude()
        {
            float sumAltitude = 0;
            Model.ForEach(x => x.Log.ForEach(y => sumAltitude += y.Altitude));
            return sumAltitude;
        }

        private ActivityData GetActivityWithLongestDistance()
        {
            var longestActivity = new ActivityData() { Distance = 0 };
            
            foreach (var tour in Model)
            {
                if (tour.Log.Count > 0)
                {
                    var result = tour.Log.OrderByDescending(x => x.Distance).First();
                    if (result.Distance > longestActivity.Distance)
                    {
                        longestActivity.Distance = result.Distance;
                        longestActivity.Date = result.EndTime;
                        longestActivity.TourName = tour.Name;
                    }
                }
            }
            return longestActivity;
        }

        private float GetSumActivityDistance()
        {
            float sumActivityDistance = 0;
            Model.ForEach(x => x.Log.ForEach(y => sumActivityDistance += y.Distance));
            return sumActivityDistance;
        }

        private double GetSumActivityTimeInHours()
        {
            var sumActivityTime = new TimeSpan(0, 0, 0);
            Model.ForEach(x => x.Log.ForEach(y => sumActivityTime = sumActivityTime.Add(y.OverallTime)));
            return sumActivityTime.TotalHours;
        }

        private int GetCountActivities()
        {
            return Model.Sum(x => x.Log.Count);
        }

        private struct ActivityData  
        {  
            public float Distance;  
            public string TourName;
            public DateTime Date;
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
