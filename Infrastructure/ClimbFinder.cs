using Altidude.Contracts.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Infrastructure
{
    public class ClimbFinder
    {
        private List<Climb> FindClimbs(TrackPoint[] points)
        {
            var climbs = new List<Climb>();

            for (int i = 0; i < points.Length; i++)
            {
                for (int j = i + 1; j < points.Length; j++)
                {
                    var distance = points[j].Distance - points[i].Distance;
                    var slope = (points[j].Altitude - points[i].Altitude) / distance;
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
