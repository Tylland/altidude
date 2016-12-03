using Altidude.Contracts.Events;
using Altidude.Contracts.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Domain.Aggregates
{
    public class ProfileAggregate : AggregateBase
    {
        public Guid UserId { get; set; }
        public Guid ChartId { get; set; }
        public string Name { get; set; }
        public Track Track { get; set; }
        public ProfilePlace[] Places { get; set; }
        public int NrOfViews { get; set; }

        public ProfileAggregate()
        {
            RegisterTransition<ProfileCreated>(Apply);
            RegisterTransition<ChartChanged>(Apply);
            RegisterTransition<ProfileViewRegistred>(Apply);
        }

        private IEnumerable<TrackPoint> FindPlacePoints(TrackPoint[] track, Place place)
        {
            List<TrackPoint> placePoints = new List<TrackPoint>();

            var i = 0;
            while (i < track.Length)
            {
                while (i < track.Length && !place.Polygon.Contains(track[i]))
                    i++;

                var clostestDistance = double.MaxValue;
                TrackPoint closestTrackpoint = null;

                while (i < track.Length && place.Polygon.Contains(track[i]))
                {
                    var distance = track[i].DistanceTo(place.Location);

                    if (clostestDistance > distance)
                    {
                        clostestDistance = distance;
                        closestTrackpoint = track[i];
                    }

                    i++;
                }

                if (closestTrackpoint != null)
                    placePoints.Add(closestTrackpoint);
            }

            return placePoints;
        }

        private int ProfilePlaceDistanceComparsion(ProfilePlace x, ProfilePlace y)
        {
            return x.Point.Distance.CompareTo(y.Point.Distance);
        }

        private ProfilePlace[] CreateProfilePlaces(Guid userId, Track track, IPlaceFinder placeFinder)
        {
            var profilePlaces = new List<ProfilePlace>();

            var places = placeFinder.Find(track.Points, userId);

            foreach (var place in places)
            {
                var placePoints = FindPlacePoints(track.Points, place);

                foreach (var placePoint in placePoints)
                    profilePlaces.Add(new ProfilePlace(place, placePoint, true, false));
            }

            profilePlaces.Sort((x, y) => { return x.Point.Distance.CompareTo(y.Point.Distance); });

            ProfilePlace startProfilePlace = null;
            ProfilePlace finishProfilePlace = null;

            var startPlace = placeFinder.FindClosest(track.FirstPoint, userId);

            if(startPlace != null)
            {
                profilePlaces.RemoveAll(pp => pp.Place.Id == startPlace.Id && pp.Point.Distance < 100);
                startProfilePlace = new ProfilePlace(startPlace, track.FirstPoint, true, true);
            }
            else
                startProfilePlace = new ProfilePlace(new Place(Guid.NewGuid(), userId, "Start", "Local", track.FirstPoint, GeoPolygon.CreateRect(track.FirstPoint, 0.05)), track.FirstPoint, true, true);


            var finishPlace = placeFinder.FindClosest(track.LastPoint, userId);

            if (finishPlace != null)
            {
                profilePlaces.RemoveAll(pp => pp.Place.Id == finishPlace.Id && pp.Point.Distance > track.Length - 100);
                finishProfilePlace = new ProfilePlace(finishPlace, track.LastPoint, true, true);
            }
            else
                finishProfilePlace = new ProfilePlace(new Place(Guid.NewGuid(), userId, "Finish", "Local", track.LastPoint, GeoPolygon.CreateRect(track.LastPoint, 0.05)), track.LastPoint, true, true);

            //var startProfilePlace = profilePlaces.FirstOrDefault(place => place.Point.Distance < 100);

            //if (startProfilePlace != null)
            //{
            //    startProfilePlace.Point.Distance = 0.0;
            //    startProfilePlace.Split = true; ;
            //}
            //else
            //    profilePlaces.Insert(0, new IProfilePlace(new Place(Guid.NewGuid(), userId, "Start", "Local", track.FirstPoint, GeoPolygon.CreateRect(track.FirstPoint, 0.05)), track.FirstPoint, true, true));

            //var finishPlace = profilePlaces.LastOrDefault(place => place.Point.Distance > (track.Length - 100));

            //if (finishPlace != null)
            //{
            //    finishPlace.Point.Distance = track.Length;
            //    finishPlace.Split = true;
            //}
            //else
            //    profilePlaces.Add(new IProfilePlace(new Place(Guid.NewGuid(), userId, "Finish", "Local", track.LastPoint, GeoPolygon.CreateRect(track.LastPoint, 0.05)), track.LastPoint, true, true));


            profilePlaces.Insert(0, startProfilePlace);
            profilePlaces.Add(finishProfilePlace);

            return profilePlaces.ToArray();
        }

        
        private Result CreateResult(User user, Track track, ProfilePlace[] places)
        {
            var splits = new List<ResultSplit>();


            ResultSplit startSplit = null;
            ResultSplit prevSplit = null;

            foreach (var place in places)
            { 
                if (place.Split)
                {
                    ResultSplit placeSplit = null;

                    if (startSplit == null)
                    {
                        startSplit = new ResultSplit(track.FirstPoint);
                        placeSplit = startSplit;
                    }

                    if(startSplit != null && prevSplit != null)
                        placeSplit = new ResultSplit(place.Point, startSplit, prevSplit);

                    splits.Add(placeSplit);

                    prevSplit = placeSplit;
                }
            }

            return new Result(new Athlete(user.Id, user.DisplayName), splits.ToArray());
        }

        private Leg CreateLeg(Track track, double startDistance, double endDistance)
        {
            var points = track.GetSubTrack(startDistance, endDistance);
            var ascending = 0.0;
            var descending = 0.0;

            for (int i = 1; i < points.Length; i++)
            {
                var altitudeDiff = points[i].Altitude - points[i - 1].Altitude;

                if (altitudeDiff < 0)
                    descending += Math.Abs(altitudeDiff);
                else
                    ascending += altitudeDiff;
            }

            return new Leg(track.CalcPointAtDistance(startDistance), track.CalcPointAtDistance(endDistance), ascending, descending);
        }
        private Leg[] CreateLegs(Track track, ProfilePlace[] places)
        {
            var legs = new List<Leg>();

            ProfilePlace prevSplitPlace = null;

            foreach(var place in places)
            {
                if (place.Split)
                {
                    if (prevSplitPlace != null)
                        legs.Add(CreateLeg(track, prevSplitPlace.Point.Distance, place.Point.Distance));

                    prevSplitPlace = place;
                }
            }

            return legs.ToArray();
        }
        public ProfileAggregate(Guid id, User user, string name, Track track, IDateTimeProvider datetime, IPlaceFinder placeFinder)
            : this()
        {
            var profilePlaces = CreateProfilePlaces(user.Id, track, placeFinder);
            var result = CreateResult(user, track, profilePlaces);
            var legs = CreateLegs(track, profilePlaces);

            RaiseEvent(new ProfileCreated(id, user.Id, name, datetime.Now, track, profilePlaces, legs, result));
        }

        public void Apply(ProfileCreated @event)
        {
            Id = @event.Id;
            UserId = @event.UserId;
            Name = @event.Name;
            Track = @event.Track;
            Places = @event.Places;
        }
        public void Apply(ChartChanged @event)
        {
            ChartId = @event.ChartId;
        }
        public void Apply(ProfileViewRegistred @event)
        {
            NrOfViews = @event.NrOfViews;
        }

        public static IAggregate Create(Guid id, User user, string name, Track track, IDateTimeProvider dateTimeProvider, IPlaceFinder placeService)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("id  can't be empty", "id");

            if (user == null)
                throw new ArgumentException("user can't be empty", "user");

            if (track.Id == Guid.Empty)
                throw new ArgumentException("track.Id can't be empty", "track.Id");


            return new ProfileAggregate(id, user, name, track, dateTimeProvider, placeService);
        }

        public void AddPlace(string name, double distance, double size, bool split)
        {
            var trackpoint = Track.CalcPointAtDistance(distance);

            RaiseEvent(new ProfilePlaceAdded(Id, UserId, name, distance, trackpoint));
        }
        public void ChangeChart(Guid chartId, string base64Image)
        {
            RaiseEvent(new ChartChanged(Id, chartId, base64Image));
        }
        public void RegisterView(string referrer)
        {
            RaiseEvent(new ProfileViewRegistred(Id, UserId, referrer, ++NrOfViews));
        }
        public void Delete(User user)
        {
            if (user == null)
                throw new ArgumentException("User does not exists");

            if (user.Id != UserId)
                throw new ArgumentException("User does not own profile");

            RaiseEvent(new ProfileDeleted(Id));
        }
    }
}
