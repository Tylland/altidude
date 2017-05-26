using System;
using Altidude.Contracts.Types;

namespace Altidude.Domain.Aggregates.Profile
{
    public class TrackData
    {
        public double Ascending { get; set; }
        public double Descending { get; set; }
        public TrackPoint HighestPoint { get; set; }
        public TrackPoint LowestPoint { get; set; }
        public Climb[] Climbs { get; set; }
    }

    public class TrackAnalyzer
    {
       
        public TrackData Analyze(Track track)
        {
            var data = new TrackData();

            TrackPoint lastPoint = null;

            foreach (var point in track.Points)
            {

                if (data.HighestPoint == null || point.Altitude > data.HighestPoint.Altitude)
                    data.HighestPoint = point;

                if (data.LowestPoint == null || point.Altitude < data.LowestPoint.Altitude)
                    data.LowestPoint = point;

                if (lastPoint != null)
                {
                    var altitudeDiff = point.Altitude - lastPoint.Altitude;

                    if (altitudeDiff < 0)
                        data.Descending += Math.Abs(altitudeDiff);
                    else
                        data.Ascending += altitudeDiff;
                }

                lastPoint = point;
            }

            data.Climbs = new ClimbFinder().Find(track.Points);
            
            return data;
        }
        
    }
}
