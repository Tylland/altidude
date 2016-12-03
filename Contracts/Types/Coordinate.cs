using System;

namespace Altidude.Contracts.Types
{
   public class Coordinate
   {
      public double X { get; set; }
      public double Y { get; set; }

      public static double CalcDistance(Coordinate from, Coordinate to)
      {
         return Math.Sqrt((to.X - from.X) * (to.X - from.X) + (to.Y - from.Y) * (to.Y - from.Y));
      }

      public double DistanceTo(Coordinate coordinate)
      {
         return CalcDistance(this, coordinate);
      }

      public Coordinate()
      {
      }

      public Coordinate(double x, double y)
      {
         X = x;
         Y = y;
      }

   }
}