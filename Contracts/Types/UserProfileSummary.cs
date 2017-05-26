using System;

namespace Altidude.Contracts.Types
{
    public class UserProfileSummary
    {
        public double Distance { get; set; }
        public double Ascending { get; set; }
        public double Descending { get; set; }
        public double HighestAltitude { get; set; }
        public double LowestAltitude { get; set; }
        public int NrOfClimbs { get; set; }
        public int ClimbPoints { get; set; }
        public int TimeSeconds { get; set; }

        public string FormatKm(Func<UserProfileSummary, double> valueFunc)
        {
            return FormatKm(valueFunc(this));
        }

        public string FormatKm(double value)
        {
            value = value / 1000.0;

            var decimals = Math.Max(2 - (int)Math.Log10(value), 0);

            return Math.Round(value, decimals) + " km";
        }
        public string FormatClimbPoints(Func<UserProfileSummary, double> valueFunc)
        {
            return FormatClimbPoints(valueFunc(this));
        }

        public string FormatClimbPoints(double value)
        {
            value = value / 8000.0;

            var decimals = Math.Max(2 - (int)Math.Log10(value), 0);

            return Math.Round(value, decimals).ToString();
        }
        public UserProfileSummary(double distance, double ascending, double descending, double highestAltitude, double lowestAltitude, int nrOfClimbs, int climbPoints, int timeSeconds)
        {
            Distance = distance;
            Ascending = ascending;
            Descending = descending;
            HighestAltitude = highestAltitude;
            LowestAltitude = lowestAltitude;
            NrOfClimbs = nrOfClimbs;
            ClimbPoints = climbPoints;
            TimeSeconds = timeSeconds;
        }
    }
}
