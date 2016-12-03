using System;

namespace Altidude.Contracts.Types
{
    public class TrackBoundary
    {
        public Guid TrackId { get; set; }
        public GeoBoundary Boundary { get; set; }
        public int OverlapCount { get; set; }

        public TrackBoundary(Guid trackId, GeoBoundary boundary, int overlapCount)
        {
            TrackId = trackId;
            Boundary = boundary;
            OverlapCount = overlapCount;
        }
        public TrackBoundary()
        {

        }


    }
}
