using Altidude.Contracts.Types;
using System;

namespace Altidude.Contracts.Events
{
    public class ProfilePlaceAdded : IEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public GeoLocation Location { get; set; }

        public ProfilePlaceAdded(Guid id, Guid userId, string name, double distance, GeoLocation location)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Distance = distance;
            Location = location;
        }
        public ProfilePlaceAdded()
        {

        }
    }
}
