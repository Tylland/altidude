using System;

namespace Altidude.Contracts.Types
{
    public class Place
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Namespace { get; set; }
        public GeoLocation Location { get; set; }
        public GeoPolygon Polygon { get; set; }
        public PlaceAttribute[] Attributes { get; set; }
        public int UsageLevel { get; set; }
        public int Rank { get; set; }

        public Place(Guid id, Guid userId, string name, string @namespace, GeoLocation location, GeoPolygon polygon, params PlaceAttribute[] attributes)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Namespace = @namespace;
            Location = location;
            Polygon = polygon;
            Attributes = attributes;
        }
    }
}
