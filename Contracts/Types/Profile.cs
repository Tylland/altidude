using System;

namespace Altidude.Contracts.Types
{
    public class Profile
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ChartId { get; set; }
        public string Name { get; set; }
        public Track Track { get; set; }
        public TrackPoint HighestPoint { get; set; }
        public TrackPoint LowestPoint { get; set; }
        public double Ascending { get; set; }
        public double Descending { get; set; }
        public Climb[] Climbs { get; set; }
        public ProfilePlace[] Places { get; set; }
        public Leg[] Legs { get; set; }
        public Result Result { get; set; }
        
        public Profile(Guid id, Guid userId, Guid chartId, string name, Track track, TrackPoint highestPoint, TrackPoint lowestPoint, double ascending, double descending, Climb[] climbs, ProfilePlace[] places, Leg[] legs, Result result)
        {
            Id = id;
            UserId = userId;
            ChartId = chartId;
            Name = name;
            Track = track;
            HighestPoint = highestPoint;
            LowestPoint = lowestPoint;
            Ascending = ascending;
            Descending = descending;
            Climbs = climbs;
            Places = places;
            Legs = legs;
            Result = result;
        }
        public Profile()
        {

        }

    }
}
