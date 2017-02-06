using System;
using Altidude.Contracts.Types;
using Altidude.Domain.Aggregates.Profile;
using GoogleMapsApi.Entities.Elevation.Request;
using GoogleMapsApi.Entities.Common;
using GoogleMapsApi;
using System.Collections.Generic;
using System.Linq;
using GoogleMapsApi.Entities.Elevation.Response;
using System.Diagnostics;
using Serilog;

namespace Altidude.Infrastructure
{

    //public class MyRequest : ElevationRequest
    //{
    //    protected override QueryStringParametersList GetQueryStringParameters()
    //    {
    //        var parameters = base.GetQueryStringParameters();

    //        return parameters;
    //    }

    //    public override Uri GetUri()
    //    {
    //        string scheme = IsSSL ? "https://" : "http://";

    //        var queryString = GetQueryStringParameters().GetQueryStringPostfix();

    //        queryString = Uri.UnescapeDataString(queryString);

    //        return new Uri(scheme + BaseUrl + "json?" + queryString);

    //        //return base.GetUri();
    //    }
    //}

    public class MyLocation : Location
    {
        public MyLocation(double lat, double lng)
            : base(lat, lng)
        {

        }
        public new string LocationString
        {
            get
            {
                var str = ToNonScientificString(Latitude) + "," + ToNonScientificString(Longitude);

                return str;
            }
        }
      
        private static string ToNonScientificString(double d)
        {

            var s = d.ToString(DoubleFormat).TrimEnd('0').Replace(',', '.');
            return s.Length == 0 ? "0.0" : s;
        }

        private static readonly string DoubleFormat = "0." + new string('#', 339);

    } 

    public class GoogleMapsElevationService : IElevationService
    {
        private static ILogger _log = Log.ForContext<GoogleMapsElevationService>();

        private const string ApiKey = "AIzaSyDXDhVp7w4sEfu0dTr4ObE0lj0YrHsEp6s";
        private const double NoAltitude = -10000;
        public double[] GetElevation(TrackPoint[] points)
        {
            var request = new ElevationRequest()
            {
                ApiKey = ApiKey,
                Locations = points.Select(p => new MyLocation(p.Latitude, p.Longitude)).ToArray()
            };

            try
            {
                var result = GoogleMaps.Elevation.Query(request);

                if (result.Status == Status.OK)
                    return result.Results.Select(res => res.Elevation).ToArray();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }


            return new double[0];
        }

        private void ApplyElevation(TrackPoint start, TrackPoint end, List<TrackPoint> points)
        {
            var distanceDelta = end.Distance - start.Distance;
            var altidudeDelta = end.Altitude - start.Altitude;

            var k = altidudeDelta / distanceDelta;

            foreach(var point in points)
                point.Altitude = start.Altitude + k * (point.Distance - start.Distance);
        }

        public void ImportElevationTo(Track track)
        {
            try
            {
                foreach (var point in track.Points)
                    point.Altitude = NoAltitude;

                var reducer = new TrackPointNumberReducer(200);

                var reducedPoints = reducer.Process(track.Points);

                var elvations = GetElevation(reducedPoints);

                for (int i = 0; i < elvations.Length; i++)
                    reducedPoints[i].Altitude = elvations[i];

                TrackPoint lastPointWithAltitude = null;
                var trackPoints = new List<TrackPoint>();

                foreach (var point in track.Points)
                {
                    if (point.Altitude != NoAltitude)
                    {
                        if (lastPointWithAltitude != null && trackPoints.Count > 0)
                        {
                            ApplyElevation(lastPointWithAltitude, point, trackPoints);
                            trackPoints.Clear();
                        }

                        lastPointWithAltitude = point;
                    }
                    else
                        trackPoints.Add(point);
                }

            }
            catch (Exception ex)
            {
                _log.Error(ex, "Import elevation to track {trackId} failed!", track.Id);
            }

        }
    }
}
