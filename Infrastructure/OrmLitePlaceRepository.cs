using Altidude.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Altidude.Contracts.Types;
using Altidude.Contracts;
using ServiceStack.OrmLite;
using System.Configuration;
using Newtonsoft.Json;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Altidude.Views;

namespace Altidude.Infrastructure
{
    public class OrmLitePlaceRepository : IPlaceRepository, IPlaceFinder, IPlaceView
    {
        private IDbConnection _db;
        private JsonSerializerSettings _serializationSettings;

        public OrmLitePlaceRepository(IDbConnection db)
        {
            _db = db;

            _serializationSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }

        private static IDbConnectionFactory CreateDbFactory(string connectionStringName)
        {
            var connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;

            return new OrmLiteConnectionFactory(connectionString, SqlServerDialect.Provider);
        }

        private string Serialize(object arg)
        {
            return JsonConvert.SerializeObject(arg, _serializationSettings);
        }
        private T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, _serializationSettings);
        }

        public void Add(Place place)
        {
            var envelope = new PlaceEnvelope();

            envelope.Id = place.Id;
            envelope.UserId = place.UserId;
            envelope.Name = place.Name;
            envelope.Namespace = place.Namespace;
            envelope.Latitude = place.Location.Latitude;
            envelope.Longitude = place.Location.Longitude;
            envelope.Altitude = place.Location.Altitude;
            envelope.BoundaryLatitudeNorth = place.Polygon.Boudary.NorthEast.Latitude;
            envelope.BoundaryLatitudeSouth = place.Polygon.Boudary.SouthWest.Latitude;
            envelope.BoundaryLongitudeEast = place.Polygon.Boudary.NorthEast.Longitude;
            envelope.BoundaryLongitudeWest = place.Polygon.Boudary.SouthWest.Longitude;
            envelope.JsonPayload = Serialize(place);
            envelope.UsageLevel = place.UsageLevel;

            _db.Insert(envelope);
        }

        public void Save(Place place)
        {
            PlaceEnvelope envelope = null;

            envelope = _db.SingleOrDefault<PlaceEnvelope>("Name = @name AND Namespace = @nspace",
                new { name = place.Name, nspace = place.Namespace });

            if (envelope == null)
                Add(place);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Place> Find(GeoLocation[] path, Guid userId)
        {
            var places = new List<Place>();

            var boundary = new GeoBoundary(path);

            // Y1' = BoundaryLatitudeSouth 
            // Y2' = BoundaryLatitudeNorth 
            // X1' = BoundaryLongitudeWest 
            // X2' = BoundaryLongitudeEast 

            //(X2' >= X1 && X1' <= X2) && (Y2' >= Y1 && Y1' <= Y2)
            //(BoundaryLongitudeEast >= @boundaryLongitudeWest && BoundaryLongitudeWest <= @boundaryLongitudeEast) && (BoundaryLatitudeNorth >= @boundaryLatitudeSouth && BoundaryLatitudeSouth <= @boundaryLatitudeNorth)

            //X1 = SouthWest.Longitude

            List<PlaceEnvelope> envelopes;

            envelopes = _db.Select<PlaceEnvelope>(envelope => (envelope.UsageLevel == PlaceUsageLevel.Public || (envelope.UsageLevel == PlaceUsageLevel.Private && envelope.UserId == userId)) 
                && (envelope.BoundaryLongitudeEast >= boundary.SouthWest.Longitude && envelope.BoundaryLongitudeWest <= boundary.NorthEast.Longitude)
                    && (envelope.BoundaryLatitudeNorth >= boundary.SouthWest.Latitude && envelope.BoundaryLatitudeSouth <= boundary.NorthEast.Latitude));

            //envelopes = db.Select<PlaceEnvelope>("(BoundaryLongitudeEast >= @boundaryLongitudeWest AND BoundaryLongitudeWest <= @boundaryLongitudeEast) AND (BoundaryLatitudeNorth >= @boundaryLatitudeSouth AND BoundaryLatitudeSouth <= @boundaryLatitudeNorth)",
            //    new
            //    {
            //        boundaryLatitudeSouth = boundary.SouthWest.Latitude,
            //        boundaryLatitudeNorth = boundary.NorthEast.Latitude,
            //        boundaryLongitudeWest = boundary.SouthWest.Longitude,
            //        boundaryLongitudeEast = boundary.NorthEast.Longitude
            //    });

            foreach (var envelope in envelopes)
            {
                var place = Deserialize<Place>(envelope.JsonPayload);

                if (place.Polygon.ContainsAny(path))
                    places.Add(place);
            }

            return places;
        }

        public List<Place> GetAll()
        {
            var envelopes = _db.Select<PlaceEnvelope>();

            return envelopes.Select(env => Deserialize<Place>(env.JsonPayload)).ToList();
        }

        public Place GetById(Guid id)
        {
            return _db.GetByIdOrDefault<Place>(id);
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Place> Find(GeoLocation location, Guid userId)
        {
            return Find(new GeoLocation[] { location }, userId);
        }

        public Place FindClosest(GeoLocation location, Guid userId)
        {
            var places = Find(location, userId);

            var closestPlace = places.MinObject(pl => pl.Location.DistanceTo(location));

            return closestPlace;
        }
    }

    public class ClosestLocationVisitor<T>
    {
        public T Closest { get; private set; }

        private double _closestDistance = double.MaxValue;
        private GeoLocation _location;

        public ClosestLocationVisitor(GeoLocation location)
        {
            _location = location;
        }

        public void Visit(T obj, GeoLocation location)
        {
            var distance = _location.DistanceTo(location);

            if (distance < _closestDistance)
            {
                _closestDistance = distance;
                Closest = obj;
            }
        }


    }

    public static class PlaceUsageLevel
    {
        public const int Unknown = 0;
        public const int Private = 1;
        public const int Public = 2;
    }

    public class PlaceEnvelope
    {
        [PrimaryKey]
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string Namespace { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double BoundaryLatitudeSouth { get; set; }
        public double BoundaryLatitudeNorth { get; set; }
        public double BoundaryLongitudeWest { get; set; }
        public double BoundaryLongitudeEast { get; set; }
        public string JsonPayload { get; set; }
        public int UsageLevel { get; set; }
    }
}
