using Altidude.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altidude.Contracts.Types;

namespace Altidude.Infrastructure
{
    public class InMemoryPlaceRepository : IPlaceRepository, IPlaceFinder
    {
        private List<Place> _places = new List<Place>();

        public void Add(Place place)
        {
            _places.Add(place);
        }

        public void Clear()
        {
            _places.Clear();
        }

        public IEnumerable<Place> Find(GeoLocation location, Guid userId)
        {
            return _places.FindAll(pl => pl.Polygon.Contains(location));
        }
        public IEnumerable<Place> Find(GeoLocation[] locations, Guid userId)
        {
            return _places.FindAll(pl => pl.Polygon.ContainsAny(locations));
        }

        public Place FindClosest(GeoLocation location, Guid userId)
        {
            var places = Find(location, userId);

            Place closestPlace = null;
            double closestDistance = double.MaxValue;

            foreach (var place in places)
            {
                var distance = place.Location.DistanceTo(location);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestPlace = place;
                }
            }

            return closestPlace;
        }

        public Place GetById(Guid id)
        {
            return _places.FirstOrDefault(p => p.Id == id);
        }

        public void Remove(Guid id)
        {
            var place = GetById(id);

            if (place != null)
                _places.Remove(place);
        }
    }
}
