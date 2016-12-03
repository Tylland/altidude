using System;

namespace Altidude.Contracts.Types
{
    public class ResultSplit
    {
        public double Distance { get; set; }
        public DateTime Time { get; set; }
        public int LegTimeSeconds { get; set; }
        public int TotalTimeSeconds { get; set; }

        public ResultSplit()
        {
        }
        public ResultSplit(TrackPoint point)
            : this(point.Distance, point.Time, 0, 0)
        {
        }
        public ResultSplit(TrackPoint point, ResultSplit startSplit, ResultSplit prevSplit)
            : this(point.Distance, point.Time, (int)point.Time.Subtract(startSplit.Time).TotalSeconds, (int)point.Time.Subtract(startSplit.Time).TotalSeconds)
        {
        }
        public ResultSplit(double distance, DateTime time, int legTimeSeconds, int totalTimeSeconds)
        {
            Distance = distance;
            Time = time;
            LegTimeSeconds = legTimeSeconds;
            TotalTimeSeconds = totalTimeSeconds;
        }
    }
}
