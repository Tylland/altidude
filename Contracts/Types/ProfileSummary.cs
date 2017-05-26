using System;

namespace Altidude.Contracts.Types
{
    public class ProfileSummary
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ChartId { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public double Ascending { get; set; }
        public double Descending { get; set; }
        public DateTime StartTime { get; set; }
        public int TimeSeconds { get; set; }
        public int NrOfViews { get; set; }
        public int Kudos { get; set; }
        public DateTime CreatedTime { get; set; }

        public ProfileSummary(Guid id, Guid userId, Guid chartId, string name, double distance, double ascending, double descending, DateTime startTime, int timeSeconds, int nrOfViews, int kudos, DateTime createdTime)
        {
            Id = id;
            UserId = userId;
            ChartId = chartId;
            Name = name;
            Distance = distance;
            Ascending = ascending;
            Descending = descending;
            StartTime = startTime;
            TimeSeconds = timeSeconds;
            NrOfViews = nrOfViews;
            Kudos = kudos;
            CreatedTime = createdTime;
        }
    }
}
