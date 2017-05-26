namespace Altidude.Contracts.Types
{
    public class Climb : TrackSegment
    {
        public ClimbCategory Category { get; set; }
        public double Slope { get; set; }
        public double Points { get; set; }


        public Climb(TrackPoint start, TrackPoint end, double ascending, double descending, ClimbCategory category, double slope, double points)
            : base(start, end, ascending, descending)
        {
            Category = category;
            Slope = slope;
            Points = points;
        }

        public Climb()
        {

        }
    }
}
