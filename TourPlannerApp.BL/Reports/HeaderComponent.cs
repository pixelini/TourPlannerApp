using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System;

namespace TourPlannerApp.BL.Reports
{
    public class HeaderComponent : IComponent
    {
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
        public byte[] Logo { get; set; }

        public HeaderComponent(string title, byte[] logo)
        {
            Title = title;
            Logo = logo;
            CreationTime = DateTime.Now;
        }

        public void Compose(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeColumn().Stack(stack =>
                {
                    stack.Item().Text($"{Title}", TextStyle.Default.Size(20).Color("FFF").Bold());
                    stack.Item().Text($"Erstellt am: {CreationTime:d}", TextStyle.Default.Color("FFF"));
                });

                if (Logo != null)
                {
                    row.ConstantColumn(100).Height(50).Image(Logo, ImageScaling.FitArea);
                } else
                {
                    row.ConstantColumn(100).Height(50).Placeholder();
                }             

            });
        }
    }

}
