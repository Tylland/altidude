using System;

namespace Altidude.Contracts.Types
{
    public class StravaActivitySummary
    {
        public long Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public string ElapsedTime { get; set; }
        public double Distance { get; set; }

        public StravaActivitySummary(long id, string type, string name, string startDate, string elapsedTime, double distance)
        {
            Id = id;
            Type = type;
            Name = name;
            StartDate = startDate;
            ElapsedTime = elapsedTime;
            Distance = distance;
        }
        public StravaActivitySummary()
        {

        }
    }
}
