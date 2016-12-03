using System;

using Altidude.Contracts.Types;
using Altidude.Contracts;

namespace Altidude.Contracts.Events
{
    public class ProfileCreated : IEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedTime { get; set; }
        public Track Track { get; set; }
        public ProfilePlace[] Places { get; set; }
        public Leg[] Legs { get; set; }
        public Result Result { get; set; }
              
        public ProfileCreated(Guid id, Guid userId, string name, DateTime createdTime, Track track, ProfilePlace[] places, Leg[] legs, Result result)
        {
            Id = id;
            UserId = userId;
            Name = name;
            CreatedTime = createdTime;
            Track = track;
            Places = places;
            Legs = legs;
            Result = result;
        }
        public ProfileCreated()
        {

        }
    }
}
