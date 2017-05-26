using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Altidude.Contracts.Commands;
using Altidude.Contracts.Events;
using Altidude.Contracts.Types;


namespace Domain.Test
{
    [TestClass]
    public class ProfileBehavior : BDDTestBase
    {
        [TestMethod]
        public void WhenCreatingProfile_ProfileShouldBeCreatedWithTheRightProerties()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var location = new GeoLocation(63.0001, 15.0001, 123.5);
            var trackPoint = new TrackPoint(location, 0.0, DateTime.Now);
            var track = new Track(Guid.NewGuid(), new TrackPoint[] { trackPoint });

            var startSplit = new ResultSplit(trackPoint);
            var result = new Result(new Athlete(userId, "IAthlete"), new ResultSplit[] { startSplit, new ResultSplit(trackPoint, startSplit, startSplit) });

            var legs = new Leg[] { new Leg(trackPoint, trackPoint, 0.0, 0.0) };

            When(new CreateProfile(id, userId, "First", track));
            Then(new ProfileCreated(id, userId, Guid.Empty, "First", DateTimeProvider.Now, track, 0.0, 0.0, trackPoint, trackPoint, null, new ProfilePlace[0], legs, result));
        }

        [TestMethod]
        public void WhenCreatingProfile_ProfileShouldBeCreatedWithOnePassedPlace()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var location = new GeoLocation(63.0001, 15.0001, 123.5);
            var trackPoint = new TrackPoint(location, 0.0, DateTime.Now);
            var track = new Track(Guid.NewGuid(), new TrackPoint[] { trackPoint });
            var place = new Place(Guid.NewGuid(), userId, "Test place", "", location, GeoPolygon.CreateRect(location, 0.05));

            var legs = new Leg[] { new Leg(trackPoint, trackPoint, 0.0, 0.0) };

            var startSplit = new ResultSplit(trackPoint);
            var result = new Result(new Athlete(userId, "IAthlete"), new ResultSplit[] { startSplit, new ResultSplit(trackPoint, startSplit, startSplit) });

            PlaceRepository.Add(place);

            var startProfilePlace = new ProfilePlace(place, trackPoint, true, true);
            var finishProfilePlace = new ProfilePlace(place, trackPoint, true, true);

            When(new CreateProfile(id, userId, "First", track));
            Then(new ProfileCreated(id, userId, Guid.Empty, "First", DateTimeProvider.Now, track, 0.0, 0.0, trackPoint, trackPoint, null, new ProfilePlace[] { startProfilePlace , finishProfilePlace }, legs, result));
        }

        [TestMethod]
        public void WhenCreatingProfile_ProfileShouldBeCreatedWithTwoPassedPlaces()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var location1 = new GeoLocation(63.0001, 15.0001, 123.5);
            var location2 = new GeoLocation(63.0051, 15.051, 125);
            var location3 = new GeoLocation(63.0011, 15.0051, 123.5);

            var now = new DateTime(2016, 08, 12);
            var trackPoint1 = new TrackPoint(location1, 0.0, now);
            var trackPoint2 = new TrackPoint(location2, 100.0, now.AddSeconds(10));
            var trackPoint3 = new TrackPoint(location3, 200.0, now.AddSeconds(20));

            var track = new Track(Guid.NewGuid(), new TrackPoint[] { trackPoint1, trackPoint2, trackPoint3 });
            var place = new Place(Guid.NewGuid(), userId, "Test place", "", location1, GeoPolygon.CreateRect(location1, 0.05));

            var legs = new Leg[] { new Leg(trackPoint1, trackPoint3, 1.5, 1.5) };

            var startSplit = new ResultSplit(trackPoint1);
            var result = new Result(new Athlete(userId, "IAthlete"), new ResultSplit[] { startSplit, new ResultSplit(trackPoint3, startSplit, startSplit) });


            PlaceRepository.Add(place);

            When(new CreateProfile(id, userId, "First", track));
            Then(new ProfileCreated(id, userId, Guid.Empty, "First", DateTimeProvider.Now, track, 1.5, 1.5, trackPoint2, trackPoint1, null, new ProfilePlace[] { new ProfilePlace(place, trackPoint1, true, true), new ProfilePlace(place, trackPoint3, true, true) }, legs, result));
        }

        [TestMethod]
        public void WhenCreatingProfile_ProfileShouldBeCreatedWithDefaultAttributes()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var location = new GeoLocation(63.0001, 15.0001, 123.5);
            var trackPoint = new TrackPoint(location, 0.0, DateTime.Now);
            var track = new Track(Guid.NewGuid(), new TrackPoint[] { trackPoint });
            var coffeeAttribute = new PlaceAttribute(PlaceAttributeType.Coffee, true);

            var place = new Place(Guid.NewGuid(), userId, "Test place", "", location, GeoPolygon.CreateRect(location, 0.05), coffeeAttribute);

            var legs = new Leg[] { new Leg(trackPoint, trackPoint, 0.0, 0.0) };

            var startSplit = new ResultSplit(trackPoint);
            var result = new Result(new Athlete(userId, "IAthlete"), new ResultSplit[] { startSplit, new ResultSplit(trackPoint, startSplit, startSplit) });

            PlaceRepository.Add(place);

            var startProfilePlace = new ProfilePlace(place, trackPoint, true, true, new ProfilePlaceAttribute(coffeeAttribute.Type, coffeeAttribute.DefaultValue));
            var finishProfilePlace = new ProfilePlace(place, trackPoint, true, true, new ProfilePlaceAttribute(coffeeAttribute.Type, coffeeAttribute.DefaultValue));

            When(new CreateProfile(id, userId, "First", track));
            Then(new ProfileCreated(id, userId, Guid.Empty, "First", DateTimeProvider.Now, track, 0.0, 0.0, trackPoint, trackPoint, null, new ProfilePlace[] { startProfilePlace, finishProfilePlace }, legs, result));
        }

        [TestMethod]
        public void WhenCreatingProfile_ProfileShouldBeCreatedWithSplits()
        {
            var id = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var location1 = new GeoLocation(63.0001, 15.0001, 123.5);
            var location2 = new GeoLocation(63.0051, 15.051, 125);
            var location3 = new GeoLocation(63.0011, 15.0051, 123.5);

            var now = new DateTime(2016, 08, 12);
            var trackPoint1 = new TrackPoint(location1, 0.0, now);
            var trackPoint2 = new TrackPoint(location2, 100.0, now.AddSeconds(10));
            var trackPoint3 = new TrackPoint(location3, 200.0, now.AddSeconds(20));

            var track = new Track(Guid.NewGuid(), new TrackPoint[] { trackPoint1, trackPoint2, trackPoint3 });
            var place = new Place(Guid.NewGuid(), userId, "Test place", "",  location2, GeoPolygon.CreateRect(location2, 0.05));

            var legs = new Leg[] { new Leg(trackPoint1, trackPoint2, 1.5, 0.0), new Leg(trackPoint2, trackPoint3, 0.0, 1.5) };

            var startSplit = new ResultSplit(trackPoint1);
            var placeSplit = new ResultSplit(trackPoint2, startSplit, startSplit);
            var finishSplit = new ResultSplit(trackPoint3, startSplit, placeSplit);

            var result = new Result(new Athlete(userId, "IAthlete"), new ResultSplit[] { startSplit, placeSplit, finishSplit });


            PlaceRepository.Add(place);

            When(new CreateProfile(id, userId, "First", track));
            Then(new ProfileCreated(id, userId, Guid.Empty, "First", DateTimeProvider.Now, track, 0.0, 0.0, trackPoint2, trackPoint1, null, new ProfilePlace[] { new ProfilePlace(place, trackPoint2, true, true) }, legs, result));
        }
    }
}
