using System;

namespace Altidude.Contracts.Commands
{
    public class ChangeChart : ICommand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ChartId { get; set; }
        public string Base64Image { get; set; }

        public ChangeChart(Guid id, Guid userId, Guid chartId, string base64Image)
        {
            Id = id;
            UserId = userId;
            ChartId = chartId;
            Base64Image = base64Image;
        }

        public ChangeChart()
        {

        }
    }
}
