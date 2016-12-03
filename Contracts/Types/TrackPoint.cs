using System;

namespace Altidude.Contracts.Types
{
    public class TrackPoint : GeoLocation
    {
        public double Distance { get; set; }
        public DateTime Time { get; set; }

        public TrackPoint() { }

        public TrackPoint(GeoLocation location, double distance, DateTime time)
        : base(location.Latitude, location.Longitude, location.Altitude)
        {
            Distance = distance;
            Time = time;
        }

        public TrackPoint(double latitude, double longitude, double altitude, double distance, DateTime time)
            : base(latitude, longitude, altitude)
        {
            Distance = distance;
            Time = time;
        }

        private static double CalcOffset(double deltaDistance, double deltaValue, double distance)
        {
            var k = deltaValue / deltaDistance;

            return k * distance;
        }

        public static TrackPoint CreateBetween(TrackPoint start, TrackPoint end, double offsetDistance)
        {
            var deltaDistance = end.Distance - start.Distance;


            var offsetLatitude = 0.0;
            var offsetLongitude = 0.0;
            var offsetAltitude = 0.0;
            var offsetTicks = 0.0;

            if (deltaDistance != 0.0)
            {
                offsetLatitude = CalcOffset(deltaDistance, end.Latitude - start.Latitude, offsetDistance);
                offsetLongitude = CalcOffset(deltaDistance, end.Longitude - start.Longitude, offsetDistance);
                offsetAltitude = CalcOffset(deltaDistance, end.Altitude - start.Altitude, offsetDistance);
                offsetTicks = CalcOffset(deltaDistance, end.Time.Ticks - start.Time.Ticks, offsetDistance);
            }

            return new TrackPoint(start.Latitude + offsetLatitude, start.Longitude + offsetLongitude, start.Altitude + offsetAltitude, start.Distance + offsetDistance, start.Time.AddTicks((long)offsetTicks));
        }

    }
}
