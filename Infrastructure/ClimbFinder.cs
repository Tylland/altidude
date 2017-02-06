using System;
using System.Collections.Generic;
using System.Linq;
using Altidude.Contracts.Types;

namespace Altidude.Infrastructure
{
    public class ClimbFinder
    {
        private double CalcSlopeGrade(TrackPoint start, TrackPoint end)
        {
            var distance = end.Distance - start.Distance;
            var rise = end.Altitude - start.Altitude;

            var run = Math.Sqrt(Math.Pow(distance, 2) - Math.Pow(rise, 2));

            return rise / run;
        }
        private double CalcAscendingDistance(TrackPoint[] points, TrackPoint start, TrackPoint end)
        {
            var segment = Track.GetSegment(points, start, end);

            TrackPoint lastPoint = null;
            var distance = 0.0;

            foreach (var trackPoint in segment)
            {
                if (lastPoint != null)
                {
                    var ascending = trackPoint.Altitude >= lastPoint.Altitude;

                    if (ascending)
                        distance += lastPoint.DistanceTo(trackPoint);
                }

                lastPoint = trackPoint;
            }

            return distance;
        }
        private List<Climb> FindClimbs(TrackPoint[] points)
        {
            var climbs = new List<Climb>();

            for (int i = 0; i < points.Length; i++)
            {
                for (int j = i + 1; j < points.Length; j++)
                {
                    var slope = CalcSlopeGrade(points[i], points[j]);
                    var distance = CalcAscendingDistance(points, points[i], points[j]);

                    var climbPoints = slope * 100 * distance;

                    var category = ClimbCategory.GetCategory(climbPoints);

                    if (category != null && slope >= ClimbCategory.MinSlope)
                        climbs.Add(new Climb(points[i], points[j], 0.0, 0.0, category, slope, climbPoints));
                }
            }

            return climbs;
        }

        public Climb[] Find(TrackPoint[] points)
        {
            var reducer = new DouglasPeuckerTransformer<TrackPoint>(10.0);

            var reducedPoints = reducer.Transform(points);

            var candidates = FindClimbs(reducedPoints);

            candidates.Sort((x, y) => y.Points.CompareTo(x.Points));

            var climbs = new List<Climb>();

            foreach (var candidate in candidates)
            {
                if (!climbs.Any(c => c.Overlap(candidate)))
                    climbs.Add(candidate);
            }

            return climbs.ToArray();
        }
    }
}
