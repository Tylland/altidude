using Altidude.Contracts.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Domain
{
    public interface IPlaceFinder
    {
        IEnumerable<Place> Find(GeoLocation location, Guid userId);
        Place FindClosest(GeoLocation location, Guid userId);
        IEnumerable<Place> Find(GeoLocation[] path, Guid userId);
    }
}
