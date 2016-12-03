using System.Collections.Generic;

namespace Altidude.Contracts.Types
{
    public class GeoBoundary
    {
        public GeoLocation NorthEast { get; set; }
        public GeoLocation SouthWest { get; set; }

        public GeoBoundary()
            : this(new GeoLocation(-90, -180), new GeoLocation(90, 180))
        {
        }

        public GeoBoundary(GeoLocation northEast, GeoLocation southWest)
        {
            NorthEast = northEast;
            SouthWest = southWest;
        }

        public GeoBoundary(IEnumerable<GeoLocation> locations)
           : this()
        {
            foreach (var location in locations)
                Include(location);
        }

        public GeoLocation GetCenter()
        {
            return new GeoLocation((NorthEast.Latitude + SouthWest.Latitude)/2, (NorthEast.Longitude + SouthWest.Longitude) / 2, (NorthEast.Altitude + SouthWest.Altitude) / 2);
        }

        public bool Contains(GeoLocation location)
        {
            return Contains(location.Latitude, location.Longitude);
        }

        public bool Contains(double latitude, double longitude)
        {
            return latitude >= SouthWest.Latitude && latitude <= NorthEast.Latitude &&
                   longitude >= SouthWest.Longitude && longitude <= NorthEast.Longitude;
        }

        public void Include(GeoLocation location)
        {
            Include(location.Latitude, location.Longitude);
        }

        public void Include(double latitude, double longitude)
        {
            if (latitude < SouthWest.Latitude)
                SouthWest.Latitude = latitude;

            if (latitude > NorthEast.Latitude)
                NorthEast.Latitude = latitude;

            if (longitude < SouthWest.Longitude)
                SouthWest.Longitude = longitude;

            if (longitude > NorthEast.Longitude)
                NorthEast.Longitude = longitude;
        }
    }
}