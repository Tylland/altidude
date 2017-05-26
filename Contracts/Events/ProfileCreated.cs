using System;

using Altidude.Contracts.Types;

namespace Altidude.Contracts.Events
{
    public class ProfileCreated : IEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ChartId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
        public Track Track { get; set; }
        public TrackPoint HighestPoint { get; set; }
        public TrackPoint LowestPoint { get; set; }
        public double Ascending { get; set; }
        public double Descending { get; set; }
        public Climb[] Climbs { get; set; }
        public ProfilePlace[] Places { get; set; }
        public Leg[] Legs { get; set; }
        public Result Result { get; set; }
              
        public ProfileCreated(Guid id, Guid userId, Guid chartId, string name, DateTime createdTime, Track track, double ascending, double descending, TrackPoint highestPoint, TrackPoint lowestPoint, Climb[] climbs, ProfilePlace[] places, Leg[] legs, Result result)
        {
            Id = id;
            UserId = userId;
            ChartId = chartId;
            Name = name;
            CreatedTime = createdTime;
            Track = track;
            Ascending = ascending;
            Descending = descending;
            HighestPoint = highestPoint;
            LowestPoint = lowestPoint;
            Climbs = climbs;
            Places = places;
            Legs = legs;
            Result = result;
        }
        public ProfileCreated()
        {

        }
    }
}
