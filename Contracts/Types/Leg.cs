using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Contracts.Types
{
    public class Leg
    {
        public TrackPoint StartPoint { get; set; }
        public TrackPoint MiddlePoint { get; set; }
        public TrackPoint EndPoint { get; set; }
        public double Length { get; set; }
        public double Ascending { get; set; }
        public double Descending { get; set; }

        public Leg(TrackPoint startPoint, TrackPoint endPoint, double ascending, double descending)
        {
            StartPoint = startPoint;
            MiddlePoint = TrackPoint.CreateBetween(startPoint, endPoint, (startPoint.Distance + endPoint.Distance) / 2.0);
            EndPoint = endPoint;
            Length = endPoint.Distance - startPoint.Distance;
            Ascending = ascending;
            Descending = descending;
        }
        public Leg()
        {

        }
    }
}
