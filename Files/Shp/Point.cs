using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Altidude.Files.Shp
{
   public struct Point
   {
      private double _x;

      public double X
      {
         get { return _x; }
         set { _x = value; }
      }

      private double _y;

      public double Y
      {
         get { return _y; }
         set { _y = value; }
      }

      private static readonly Point _empty = new Point(0, 0);

      public static Point Empty
      {
         get { return Point._empty; }
      }


      private static readonly Point _zero = new Point(0, 0);

      public static Point Zero
      {
         get { return Point._zero; }
      }

      public Point(double x, double y)
      {
         _x = x;
         _y = y;
      }
   }
}
