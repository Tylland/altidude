using Altidude.Contracts.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Infrastructure
{
    public class ReduceSegment
    {
        public double maxDistance = 0.0;
        public int maxDistanceIndex = 0;
        public TrackPoint maxDistancePoint = null;

        public bool Splitted = false;
        public ReduceSegment firstSubSegment = null;
        public ReduceSegment secondSubSegment = null;

        public List<TrackPoint> Points;

        public void Split()
        {
            firstSubSegment = new ReduceSegment(Points.Take(maxDistanceIndex + 1).ToArray());
            secondSubSegment = new ReduceSegment(Points.Skip(maxDistanceIndex).ToArray());

            Splitted = true;
        }

        private double AltitudeDifferenceTo(TrackPoint point, TrackPoint start, TrackPoint end)
        {
            var k = (end.Altitude - start.Altitude) / (end.Distance - start.Distance);

            var altitude = start.Altitude + k * (point.Distance - start.Distance);

            return Math.Abs(point.Altitude - altitude);
        }

        public ReduceSegment(TrackPoint[] points)
        {
            Points = new List<TrackPoint>(points);

            for (var i = 1; i < Points.Count - 1; i++)
            {
                var distance = AltitudeDifferenceTo(Points[i], Points.First(), Points.Last());

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxDistanceIndex = i;
                    maxDistancePoint = Points[i];
                }
            }

            if (maxDistancePoint == null)
            {
                maxDistanceIndex = (int)Math.Floor(Points.Count / 2.0);
                maxDistancePoint = Points[maxDistanceIndex];
            }
        }

    }

    public class TrackPointNumberReducer
    {
        public int MaxNrOfPoints { get; private set; }

        public TrackPoint[] Process(TrackPoint[] points)
        {
            List<TrackPoint> result = new List<TrackPoint>();

            if (points.Length <= MaxNrOfPoints)
                return points;

            var rootSegment = new ReduceSegment(points);

            for (var i = 0; i < MaxNrOfPoints; i++)
            {
                var segment = FindSegmentWithMaxDistance(rootSegment);

                segment.Split();
            }

            result.Add(rootSegment.Points.First());
            result.AddRange(GetPoints(rootSegment));
            result.Add(rootSegment.Points.Last());

            return result.ToArray();
        }

        private List<TrackPoint> GetPoints(ReduceSegment segment)
        {
            var points = new List<TrackPoint>();

            if (!segment.Splitted)
                return points;

            points.AddRange(GetPoints(segment.firstSubSegment));
            points.Add(segment.maxDistancePoint);
            points.AddRange(GetPoints(segment.secondSubSegment));

            return points;
        }

        private ReduceSegment FindSegmentWithMaxDistance(ReduceSegment segment)
        {
            if (!segment.Splitted)
                return segment;

            var firstSegment = FindSegmentWithMaxDistance(segment.firstSubSegment);
            var secondSegment = FindSegmentWithMaxDistance(segment.secondSubSegment);

            if (firstSegment.maxDistance >= secondSegment.maxDistance)
                return firstSegment;

            return secondSegment;
        }

        public TrackPointNumberReducer(int maxNrOfPoints)
        {
            MaxNrOfPoints = maxNrOfPoints;
        }
    }
}
