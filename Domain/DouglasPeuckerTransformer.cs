using System;
using System.Collections.Generic;
using System.Linq;
using Altidude.Contracts.Types;

namespace Altidude.Domain
{

   //   function DouglasPeucker(PointList[], epsilon)
   //    // Find the point with the maximum distance
   //    dmax = 0
   //    index = 0
   //    for i = 2 to (length(PointList) - 1)
   //        d = PerpendicularDistance(PointList[i], Line(PointList[1], PointList[end])) 
   //        if d > dmax
   //            index = i
   //            dmax = d

   //    // If max distance is greater than epsilon, recursively simplify
   //    if dmax >= epsilon
   //        // Recursive call
   //        recResults1[] = DouglasPeucker(PointList[1...index], epsilon)
   //        recResults2[] = DouglasPeucker(PointList[index...end], epsilon)

   //        // Build the result list
   //        ResultList[] = {recResults1[1...end-1] recResults2[1...end]}
   //    else
   //        ResultList[] = {PointList[1], PointList[end]}

   //    // Return the result
   //    return ResultList[]
   //end

   public class DouglasPeuckerTransformer<T> where T : TrackPoint
   {
      private const double MinEpsilon = 0.00001;
      private const int MaxLevels = 1000;

      private readonly double _epsilon;
      private readonly int _maxlevels = 10000;


      private double CalcPerpendicularDistance(T location, T start, T end)
      {
         double A = location.Latitude - start.Latitude;
         double B = location.Longitude - start.Longitude;
         double C = end.Latitude - start.Latitude;
         double D = end.Longitude - start.Longitude;

         return Math.Abs(A * D - C * B) / Math.Sqrt(C * C + D * D);
      }

        private double CalcAltitudeDistance(T point, T start, T end)
        {
            var k = (end.Altitude - start.Altitude) / (end.Distance - start.Distance);

            var altitude = start.Altitude + k * (point.Distance - start.Distance);

            return Math.Abs(point.Altitude - altitude);
        }

        private T[] GetSubArray(T[] positions, int start, int end)
      {
         int length = (end - start) + 1;

         var subset = new T[length];

         Array.Copy(positions, start, subset, 0, length);

         return subset;
      }

      private T[] DouglasPeucker(T[] positions, int level)
      {
         double maxDistance = 0.0;
         int maxAltitudeIndex = 0;

         for (int i = 1; i < positions.Length - 2; i++)
         {
            double distance = CalcAltitudeDistance(positions[i], positions[0], positions[positions.Length - 1]);

            if (distance > maxDistance)
            {
               maxDistance = distance;
               maxAltitudeIndex = i;
            }
         }

         var result = new List<T>();

         if (level <= _maxlevels && maxDistance > _epsilon)
         {
            var result1 = DouglasPeucker(GetSubArray(positions, 0, maxAltitudeIndex), ++level);
            var result2 = DouglasPeucker(GetSubArray(positions, maxAltitudeIndex, positions.Length - 1), ++level);

            result.AddRange(GetSubArray(result1, 0, result1.Length - 2));
            result.AddRange(result2);
         }
         else
         {
            result.Add(positions[0]);
            result.Add(positions[positions.Length - 1]);
         }

         return result.ToArray();
      }

      public T[] Transform(T[] positions)
      {
         return DouglasPeucker(positions, 1);
      }

      public IList<T> Transform(IList<T> locations)
      {
         return new List<T>(DouglasPeucker(locations.ToArray(), 1));
      }

      //public IList<T> Transform(IList<T> positions, IList<IGeoLocation> fixedPositions)
      //{
      //   var transformedPositions = new List<T>();

      //   IGeoLocation last = null;

      //   foreach (var item in fixedPositions)
      //   {
      //      if (last != null)
      //      {
      //         var segment = positions.SkipWhile(pos => pos.Latitude != last.Latitude && pos.Longitude != last.Longitude).TakeWhilePrevious(pos => pos.Latitude != item.Latitude && pos.Longitude != item.Longitude).ToArray();

      //         if (segment.Length > 2)
      //         {
      //            transformedPositions.AddRange(DouglasPeucker(segment, 2));
      //         }
      //      }

      //      last = item;
      //   }

      //   return transformedPositions;
      //}

      public DouglasPeuckerTransformer(double epsilon)
      {
         _epsilon = epsilon;
         _maxlevels = MaxLevels;
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="maxlevels">Max number of recursion. Constraints number of transformed points to 2 to the power of maxlevels</param>
      public DouglasPeuckerTransformer(int maxlevels)
      {
         _epsilon = MinEpsilon;
         _maxlevels = maxlevels;
      }

   }
}