namespace Altidude.Transformation
{
   public class Ellipsoid
   {
      // Semi-major axis of the ellipsoid.
      public double Axis { get; set; }
      // Flattening of the ellipsoid.
      public double Flattening { get; set; }
      // Central meridian for the projection.    
      public double CentralMeridian { get; set; }
      // Scale on central meridian.
      public double Scale { get; set; }
      // Offset for origo.
      public double FalseNorthing { get; set; }
      // Offset for origo. 
      public double FalseEasting { get; set; }

      public Ellipsoid(double axis, double flattening, double centralMeridian, double scale, double falseNorthing, double falseEasting)
      {
         Axis = axis;
         Flattening = flattening;
         CentralMeridian = centralMeridian;
         Scale = scale;
         FalseNorthing = falseNorthing;
         FalseEasting = falseEasting;
      }

      public static readonly Ellipsoid WGS84 = new Ellipsoid(6378137.0, 1 / 298.257223563, 0.0, 1.0, 0.0, 0.0);
   }
}