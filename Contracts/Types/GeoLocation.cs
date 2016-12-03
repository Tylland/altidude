using System;
using System.Linq;

namespace Altidude.Contracts.Types
{
    public class GeoLocation : ValueObject
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }

        public static double CalcDistance(GeoLocation from, GeoLocation to)
        {
            return Math.Sqrt((to.Latitude - from.Latitude) * (to.Latitude - from.Latitude) + (to.Longitude - from.Longitude) * (to.Longitude - from.Longitude));
        }

        public double DistanceTo(GeoLocation location)
        {
            return CalcDistance(this, location);
        }
        public double ShortestDistanceTo(GeoLocation[] path)
        {
            return path.Min(loc => loc.DistanceTo(this));
        }

        public GeoLocation Offset(double deltaLatitude, double deltaLongitude, double deltaAltitide)
        {
            return new GeoLocation(Latitude + deltaLatitude, Longitude + deltaLongitude, Altitude + deltaAltitide);
        }

        public GeoLocation(double latitude, double longitude)
            : this(latitude, longitude, 0.0)
        {
        }

        public GeoLocation(double latitude, double longitude, double altitude)
        {
            Latitude = latitude;
            Longitude = longitude;
            Altitude = altitude;
        }
        public GeoLocation()
        {

        }
    }
}
