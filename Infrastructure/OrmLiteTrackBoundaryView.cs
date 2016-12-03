using Altidude.Views;
using System;
using System.Linq;
using Altidude.Contracts.Events;
using ServiceStack.OrmLite;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Altidude.Contracts.Types;
using ServiceStack.DataAnnotations;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Altidude.Contracts;
using System.Data;
using System.Collections.Generic;
using Altidude.Domain;

namespace Altidude.Infrastructure
{
    public class OrmLiteTrackBoundaryView : ITrackBoundaryView, IHandleEvent<ProfileCreated>//, IHandleEvent<ChartChanged>
    {
        private IDbConnection _db;

        public OrmLiteTrackBoundaryView(IDbConnection db)
        {
            _db = db;
        }

        private TrackBoundary CreateTrackBoundary(TrackBoundaryView view)
        {
            return new TrackBoundary(view.TrackId, new GeoBoundary(new GeoLocation(view.North, view.East), new GeoLocation(view.South, view.West)), view.OverlapCount);
        }
        public List<TrackBoundaryView> GetOverlapping(GeoBoundary boundary)
        {
            var views = _db.Select<TrackBoundaryView>(view =>
                (view.East >= boundary.SouthWest.Longitude && view.West <= boundary.NorthEast.Longitude)
                    && (view.North >= boundary.SouthWest.Latitude && view.South <= boundary.NorthEast.Latitude));

            Debug.WriteLine(_db.GetLastSql());

            return views;
        }

        public void Handle(ProfileCreated evt)
        {
            var boundary = new GeoBoundary(evt.Track.Points);

            var overlappingViews = GetOverlapping(boundary);

            foreach (var overlappingView in overlappingViews)
            {
                overlappingView.OverlapCount++;

                _db.Update(overlappingView);
            }

            var view = new TrackBoundaryView();
            view.TrackId = evt.Track.Id;
            view.North = boundary.NorthEast.Latitude;
            view.East = boundary.NorthEast.Longitude;
            view.South = boundary.SouthWest.Latitude;
            view.West = boundary.SouthWest.Longitude;
            view.OverlapCount = overlappingViews.Count;

            _db.Insert(view);
        }

    }
    public class TrackBoundaryView
    {
        [PrimaryKey]
        public Guid TrackId { get; set; }
        public double South { get; set; }
        public double North { get; set; }
        public double West { get; set; }
        public double East { get; set; }
        public int OverlapCount { get; set; }
    }
}
