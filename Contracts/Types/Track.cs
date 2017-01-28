using System;
using System.Linq;


namespace Altidude.Contracts.Types
{
    public class Track : ValueObject
    {
        public Guid Id { get; set; }

        public TrackPoint[] Points { get; set; }

        public Climb[] Climbs { get; set; }

        public TrackPoint FirstPoint
        {
            get { return Points[0]; }
        }
        public TrackPoint LastPoint
        {
            get { return Points[Points.Length - 1]; }
        }

        public double Length
        {
            get
            {
                return Points.Length > 0 ? LastPoint.Distance : 0.0;
            }
        }
        public TrackPoint[] GetSubTrack(double startDistance, double endDistance)
        {
            return Points.SkipWhile(point => point.Distance < startDistance).TakeWhile(point => point.Distance <= endDistance).ToArray();

            //var start = GetPointAtDistance(startDistance);
            //var end = GetPointAtDistance(endDistance);

            //return GetSubTrack(start, end);
        }

        public TrackPoint[] GetSubTrack(TrackPoint start, TrackPoint end)
        {
            return Points.SkipWhile(point => point != start).TakeWhile(point => point != end).ToArray();
        }

        public TrackPoint FindClosestPointAtDistance(double distance)
        {
            TrackPoint closestPoint = null;

            var closestDelta = double.MaxValue;

            foreach(var point in Points)
            {
                var delta = Math.Abs(point.Distance - distance);

                if(delta < closestDelta)
                {
                    closestPoint = point;
                    closestDelta = delta;
                }
            }

            return closestPoint;
        }

        public TrackPoint CalcPointAtDistance(double distance)
        {
            for (var i = 1; i < Points.Length; i++)
            {
                var start = Points[i - 1];
                var end = Points[i];

                if (start.Distance <= distance && distance <= end.Distance)
                {
                    return TrackPoint.CreateBetween(start, end, distance - start.Distance);
                }
            }

            return LastPoint;
        }
        public Track(Guid id, TrackPoint[] points, Climb[] climbs)
        {
            Id = id;
            Points = points;
            Climbs = climbs;
        }
        public Track(Guid id, TrackPoint[] points)
        {
            Id = id;
            Points = points;
        }
        public Track()
        {

        }
    }
}
