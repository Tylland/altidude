using Altidude.Contracts.Types;

namespace Altidude.Domain.Aggregates.Profile
{
    public interface IElevationService
    {
        double[] GetElevation(TrackPoint[] locations);
        void ImportElevationTo(Track track);
    }
}
