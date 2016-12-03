using System.Collections.Generic;
using System.Linq;

namespace Altidude.Contracts.Types
{
    public class ProfilePlace 
    {
        public Place Place { get; set; }
        public TrackPoint Point { get; set; }
        public bool Active { get; set; }
        public bool Split { get; set; }

        public ProfilePlaceAttribute[] Attributes { get; set; }

        public ProfilePlaceAttribute GetAttribute(PlaceAttributeType type)
        {
            return Attributes.FirstOrDefault(attr => attr.Type.Equals(type));
        }

        public bool GetAttributeValue(PlaceAttributeType type)
        {
            var attribute = GetAttribute(type);

            return attribute != null && attribute.Value;
        }

        private static ProfilePlaceAttribute[] CreateDefaultAttributes(Place place)
        {
            if (place.Attributes == null)
                return new ProfilePlaceAttribute[0];

            var attributes = new List<ProfilePlaceAttribute>();

            foreach (var placeAttribute in place.Attributes)
                attributes.Add(new ProfilePlaceAttribute(placeAttribute.Type, placeAttribute.DefaultValue));

            return attributes.ToArray();
        }
        public ProfilePlace(Place place, TrackPoint point, bool active, bool split)
            : this(place, point, active, split, CreateDefaultAttributes(place))
        {
        
        }
        public ProfilePlace(Place place, TrackPoint point, bool active, bool split, params ProfilePlaceAttribute[] attributes)
        {
            Place = place;
            Point = point;
            Active = active;
            Attributes = attributes;
            Split = split;
        }

        public ProfilePlace()
        {

        }
    }
}
