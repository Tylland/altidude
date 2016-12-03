using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Altidude.Contracts.Types;

namespace Altidude.Transformation
{
   public class GaussKreugerTranformation
   {
      private Ellipsoid _ellipsoid;

      public GaussKreugerTranformation(Ellipsoid ellipsoid)
      {
         _ellipsoid = ellipsoid;
      }

      public Coordinate ToCoordinate(double latitude, double longitude)
      {
         return Transform(new GeoLocation(latitude, longitude));
      }

      // Conversion from geodetic coordinates to grid coordinates.
      public Coordinate Transform(GeoLocation location)
      {
         // Prepare ellipsoid-based stuff.
         double e2 = _ellipsoid.Flattening * (2.0 - _ellipsoid.Flattening);
         double n = _ellipsoid.Flattening / (2.0 - _ellipsoid.Flattening);
         double a_roof = _ellipsoid.Axis / (1.0 + n) * (1.0 + n * n / 4.0 + n * n * n * n / 64.0);
         double A = e2;
         double B = (5.0 * e2 * e2 - e2 * e2 * e2) / 6.0;
         double C = (104.0 * e2 * e2 * e2 - 45.0 * e2 * e2 * e2 * e2) / 120.0;
         double D = (1237.0 * e2 * e2 * e2 * e2) / 1260.0;
         double beta1 = n / 2.0 - 2.0 * n * n / 3.0 + 5.0 * n * n * n / 16.0 + 41.0 * n * n * n * n / 180.0;
         double beta2 = 13.0 * n * n / 48.0 - 3.0 * n * n * n / 5.0 + 557.0 * n * n * n * n / 1440.0;
         double beta3 = 61.0 * n * n * n / 240.0 - 103.0 * n * n * n * n / 140.0;
         double beta4 = 49561.0 * n * n * n * n / 161280.0;

         // Convert.
         double deg_to_rad = Math.PI / 180.0;
         double phi = location.Latitude * deg_to_rad;
         double lambda = location.Longitude * deg_to_rad;
         double lambda_zero = _ellipsoid.CentralMeridian * deg_to_rad;

         double phi_star = phi - Math.Sin(phi) * Math.Cos(phi) * (A +
                         B * Math.Pow(Math.Sin(phi), 2) +
                         C * Math.Pow(Math.Sin(phi), 4) +
                         D * Math.Pow(Math.Sin(phi), 6));
         double delta_lambda = lambda - lambda_zero;
         double xi_prim = Math.Atan(Math.Tan(phi_star) / Math.Cos(delta_lambda));
         double eta_prim = math_atanh(Math.Cos(phi_star) * Math.Sin(delta_lambda));
         double x = _ellipsoid.Scale * a_roof * (xi_prim +
                         beta1 * Math.Sin(2.0 * xi_prim) * math_cosh(2.0 * eta_prim) +
                         beta2 * Math.Sin(4.0 * xi_prim) * math_cosh(4.0 * eta_prim) +
                         beta3 * Math.Sin(6.0 * xi_prim) * math_cosh(6.0 * eta_prim) +
                         beta4 * Math.Sin(8.0 * xi_prim) * math_cosh(8.0 * eta_prim)) +
                         _ellipsoid.FalseNorthing;
         double y = _ellipsoid.Scale * a_roof * (eta_prim +
                         beta1 * Math.Cos(2.0 * xi_prim) * math_sinh(2.0 * eta_prim) +
                         beta2 * Math.Cos(4.0 * xi_prim) * math_sinh(4.0 * eta_prim) +
                         beta3 * Math.Cos(6.0 * xi_prim) * math_sinh(6.0 * eta_prim) +
                         beta4 * Math.Cos(8.0 * xi_prim) * math_sinh(8.0 * eta_prim)) +
                         _ellipsoid.FalseEasting;

         return new Coordinate(Math.Round(x * 1000.0) / 1000.0, Math.Round(y * 1000.0) / 1000.0);
      }

      public GeoLocation ToLocation(double x, double y)
      {
         return Transform(new Coordinate(x, y));
      }

      // Conversion from grid coordinates to geodetic coordinates.
      public GeoLocation Transform(Coordinate coordinate)
      {
         if (_ellipsoid.CentralMeridian == double.MinValue)
         {
            return new GeoLocation();
         }

         // Prepare ellipsoid-based stuff.
         double e2 = _ellipsoid.Flattening * (2.0 - _ellipsoid.Flattening);
         double n = _ellipsoid.Flattening / (2.0 - _ellipsoid.Flattening);
         double a_roof = _ellipsoid.Axis / (1.0 + n) * (1.0 + n * n / 4.0 + n * n * n * n / 64.0);
         double delta1 = n / 2.0 - 2.0 * n * n / 3.0 + 37.0 * n * n * n / 96.0 - n * n * n * n / 360.0;
         double delta2 = n * n / 48.0 + n * n * n / 15.0 - 437.0 * n * n * n * n / 1440.0;
         double delta3 = 17.0 * n * n * n / 480.0 - 37 * n * n * n * n / 840.0;
         double delta4 = 4397.0 * n * n * n * n / 161280.0;

         double Astar = e2 + e2 * e2 + e2 * e2 * e2 + e2 * e2 * e2 * e2;
         double Bstar = -(7.0 * e2 * e2 + 17.0 * e2 * e2 * e2 + 30.0 * e2 * e2 * e2 * e2) / 6.0;
         double Cstar = (224.0 * e2 * e2 * e2 + 889.0 * e2 * e2 * e2 * e2) / 120.0;
         double Dstar = -(4279.0 * e2 * e2 * e2 * e2) / 1260.0;

         // Convert.
         double deg_to_rad = Math.PI / 180;
         double lambda_zero = _ellipsoid.CentralMeridian * deg_to_rad;
         double xi = (coordinate.X - _ellipsoid.FalseNorthing) / (_ellipsoid.Scale * a_roof);
         double eta = (coordinate.Y - _ellipsoid.FalseEasting) / (_ellipsoid.Scale * a_roof);
         double xi_prim = xi -
                         delta1 * Math.Sin(2.0 * xi) * math_cosh(2.0 * eta) -
                         delta2 * Math.Sin(4.0 * xi) * math_cosh(4.0 * eta) -
                         delta3 * Math.Sin(6.0 * xi) * math_cosh(6.0 * eta) -
                         delta4 * Math.Sin(8.0 * xi) * math_cosh(8.0 * eta);
         double eta_prim = eta -
                         delta1 * Math.Cos(2.0 * xi) * math_sinh(2.0 * eta) -
                         delta2 * Math.Cos(4.0 * xi) * math_sinh(4.0 * eta) -
                         delta3 * Math.Cos(6.0 * xi) * math_sinh(6.0 * eta) -
                         delta4 * Math.Cos(8.0 * xi) * math_sinh(8.0 * eta);
         double phi_star = Math.Asin(Math.Sin(xi_prim) / math_cosh(eta_prim));
         double delta_lambda = Math.Atan(math_sinh(eta_prim) / Math.Cos(xi_prim));
         double lon_radian = lambda_zero + delta_lambda;
         double lat_radian = phi_star + Math.Sin(phi_star) * Math.Cos(phi_star) *
                         (Astar +
                          Bstar * Math.Pow(Math.Sin(phi_star), 2) +
                          Cstar * Math.Pow(Math.Sin(phi_star), 4) +
                          Dstar * Math.Pow(Math.Sin(phi_star), 6));

         return new GeoLocation(lat_radian * 180.0 / Math.PI, lon_radian * 180.0 / Math.PI);
      }


      private double math_sinh(double value)
      {
         return 0.5 * (Math.Exp(value) - Math.Exp(-value));
      }
      private double math_cosh(double value)
      {
         return 0.5 * (Math.Exp(value) + Math.Exp(-value));
      }
      private double math_atanh(double value)
      {
         return 0.5 * Math.Log((1.0 + value) / (1.0 - value));
      }

   }
}