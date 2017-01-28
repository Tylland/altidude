using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Contracts.Types
{
    public class TrackSegment
    {
        public TrackPoint Start { get; set; }
        public TrackPoint End { get; set; }
        public double Length { get; set; }
        public double Ascending { get; set; }
        public double Descending { get; set; }

        public bool Overlap(Climb climb)
        {
            return climb.Start.Distance >= Start.Distance && climb.Start.Distance <= End.Distance || climb.End.Distance >= Start.Distance && climb.End.Distance <= End.Distance || Start.Distance >= climb.Start.Distance && Start.Distance <= climb.End.Distance || End.Distance >= climb.Start.Distance && End.Distance <= climb.End.Distance;
        }

        public TrackSegment(TrackPoint start, TrackPoint end, double ascending, double descending)
        {
            Start = start;
            End= end;
            Length = end.Distance - start.Distance;
            Ascending = ascending;
            Descending = descending;
        }
        public TrackSegment()
        {

        }

    }
}