using System.Linq;

namespace Altidude.Contracts.Types
{
    public class GeoPolygon
    {
        public GeoLocation[] Locations { get; private set; }
        public GeoBoundary Boudary { get; private set; }

        public GeoPolygon(params GeoLocation[] locations)
        {
            Locations = locations;
            Boudary = new GeoBoundary(Locations);
        }

        public bool ContainsAny(GeoLocation[] locations)
        {
            return locations.Any(location => Contains(location));
        }
        public bool Contains(GeoLocation location)
        {
            if (!Boudary.Contains(location))
                return false;

            // http://www.ecse.rpi.edu/Homepages/wrf/Research/Short_Notes/pnpoly.html
            bool inside = false;
            for (int i = 0, j = Locations.Length - 1; i < Locations.Length; j = i++)
            {
                if ((Locations[i].Longitude > location.Longitude) != (Locations[j].Longitude > location.Longitude) &&
                     location.Latitude < (Locations[j].Latitude - Locations[i].Latitude) * (location.Longitude - Locations[i].Longitude) / (Locations[j].Longitude - Locations[i].Longitude) + Locations[i].Latitude)
                {
                    inside = !inside;
                }
            }

            return inside;
        }

        public static GeoPolygon CreateRect(GeoLocation center, double sizeDegrees)
        {
            var delta = sizeDegrees / 2.0;

            return new GeoPolygon(center.Offset(delta, -delta, 0), center.Offset(delta, delta, 0), center.Offset(-delta, delta, 0), center.Offset(-delta, -delta, 0), center.Offset(delta, -delta, 0));
        }
    }
}