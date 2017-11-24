using Altidude.Contracts.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Altidude.Contracts.Commands;
using Altidude.Domain.Aggregates.Profile;
using Altidude.Files;
using tcx20 = Altidude.Files.Tcx20;
using gpx11 = Altidude.Files.Gpx11;
using Altidude.Transformation;
using Altidude.Infrastructure;

namespace Altidude.net.Controllers
{
    public class ApiProfileController : BaseApiController
    {
        [HttpGet]
        [Route("api/v1/profiles/{profileId}")]
        public Profile GetProfile([FromUri]Guid profileId)
        {
            var views = ApplicationManager.BuildViews();

            return views.Profiles.GetById(profileId);
        }

        [HttpGet]
        [Route("api/v1/profiles/latests/{nrOfProfiles}")]
        public List<Profile> GetLatestProfilea([FromUri]int nrOfProfiles)
        {
            var views = ApplicationManager.BuildViews();

            return views.Profiles.GetLatest(nrOfProfiles);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v1/profiles/{profileId}/changechart")]
        public void ChangeChart([FromUri]Guid profileId, [FromBody]ChangeChart command)
        {
            var application = ApplicationManager.BuildApplication();

            application.ExecuteCommand(new ChangeChart(profileId, Guid.Empty, command.ChartId, command.Base64Image));
        }
        [HttpPost]
        [Authorize]
        [Route("api/v1/profiles/{profileId}/givekudos")]
        public HttpResponseMessage GiveKudos([FromUri]Guid profileId)
        {
            if (!HttpContext.Current.Request.IsAuthenticated)
                return Request.CreateResponse(HttpStatusCode.Unauthorized);

            var userId = new Guid(User.Identity.GetUserId());

            var application = ApplicationManager.BuildApplication();

            application.ExecuteCommand(new GiveKudos(profileId, userId));

            var summary = application.Views.Profiles.GetSummaryById(profileId);

            if (summary != null)
                return Request.CreateResponse(HttpStatusCode.OK, summary.Kudos);

            return Request.CreateResponse(HttpStatusCode.NotFound);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v1/profiles/{profileId}/delete")]
        public void Delete([FromUri]Guid profileId)
        {
            var userId = new Guid(User.Identity.GetUserId());

            var application = ApplicationManager.BuildApplication();

            application.ExecuteCommand(new DeleteProfile(profileId, userId));
        }


        public static Track CreateTrack(tcx20.TrainingCenterDatabase_t database)
        {
            var trackPoints = new List<TrackPoint>();

            var track = new List<tcx20.Trackpoint_t>();

            if (database.Activities != null && database.Activities.Activity != null &&
                database.Activities.Activity.Length > 0)
            {
                var activity = database.Activities.Activity[0];

                foreach (var lap in activity.Lap)
                    track.AddRange(lap.Track);

            }
            else if (database.Courses != null && database.Courses.Length > 0 &&
                     database.Courses[0].Track != null && database.Courses[0].Track.Length > 0)
            {
                track.AddRange(database.Courses[0].Track);
            }


            if (track.Count <= 0)
                return null;

            var index = 0;
            var firstAltitudeIndex = -1;
            var distance = 0.0;
            var altitude = -1.0;

            TrackPoint firstPoint = null;

            foreach (tcx20.Trackpoint_t point in track)
            {
                if (point.AltitudeMetersSpecified)
                {
                    if (firstAltitudeIndex == -1)
                        firstAltitudeIndex = index;

                    altitude = point.AltitudeMeters;
                }

                if (point.DistanceMetersSpecified)
                    distance = point.DistanceMeters;

                if (point.Position != null)
                {
                    var trackpoint = new TrackPoint(point.Position.LatitudeDegrees, point.Position.LongitudeDegrees,
                        altitude, distance, point.Time);

                    if (firstPoint == null)
                        firstPoint = trackpoint;

                    trackPoints.Add(trackpoint);

                    index++;
                }

            }

            for (var i = firstAltitudeIndex; i >= 0; i--)
                trackPoints[i].Altitude = trackPoints[firstAltitudeIndex].Altitude;

            var climbs = new ClimbFinder().Find(trackPoints.ToArray());

            return new Track(Guid.NewGuid(), trackPoints.ToArray(), climbs);
        }

        public static Track CreateTrack(gpx11.gpxType gpx)
        {
            var trackPoints = new List<TrackPoint>();

            var transformer = new GaussKreugerTranformation(Ellipsoid.WGS84);

            if (gpx.trk.Length > 0)
            {
                var trk = gpx.trk[0];

                double distance = 0.0;
                Coordinate lastCoordinate = null;

                foreach (var trkseg in trk.trkseg)
                {
                    foreach (var trkpt in trkseg.trkpt)
                    {
                        var coodinate = transformer.ToCoordinate((double)trkpt.lat, (double)trkpt.lon);

                        if (lastCoordinate != null)
                            distance += lastCoordinate.DistanceTo(coodinate);

                        lastCoordinate = coodinate;

                        var trackpoint = new TrackPoint((double)trkpt.lat, (double)trkpt.lon, (double)trkpt.ele, distance, trkpt.time);

                        trackPoints.Add(trackpoint);
                    }
                }
            }

            var climbs = new ClimbFinder().Find(trackPoints.ToArray());


            return new Track(Guid.NewGuid(), trackPoints.ToArray(), climbs);
        }

        private string GetNameFromFilename(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                return string.Empty;

            var name = Path.GetFileNameWithoutExtension(filename);
            name = name.Replace("_", " ");

            return char.ToUpper(name[0]) + name.Substring(1);
        }

        [HttpPost]
        [Authorize]
        [Route("api/v1/profiles/upload/trackfile/chart/{chartd}")]
        public async System.Threading.Tasks.Task<Profile> UploadTrackfile([FromUri]Guid chartId)
        {
            if (!Request.Content.IsMimeMultipartContent())
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);

            var provider = new MultipartMemoryStreamProvider();

            await Request.Content.ReadAsMultipartAsync(provider);

            foreach (var file in provider.Contents)
            {
                var filename = file.Headers.ContentDisposition.FileName.Trim('\"');

                var ext = Path.GetExtension(filename).ToLower();

                Track track = null;

                if (ext.Equals(".tcx"))
                {
                    var database = Tcx20Reader.Open(await file.ReadAsStreamAsync());

                    track = CreateTrack(database);
                }

                if (ext.Equals(".gpx"))
                {
                    var gpx = Gpx11Reader.Open(await file.ReadAsStreamAsync());

                    track = CreateTrack(gpx);
                }

                if (track != null)
                {
                    var id = Guid.NewGuid();
                    var name = GetNameFromFilename(filename);

                    var application = ApplicationManager.BuildApplication();

                    application.ExecuteCommand(new CreateProfile(id, UserId, chartId, name, track));

                    return application.Views.Profiles.GetById(id);
                }

            }

            return null;
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/profiles/gpx")]
        public HttpResponseMessage GetProfilePositionFile()
        {
            var views = ApplicationManager.BuildViews();

            var profiles = views.Profiles.GetAll();

            var waypoints = new List<gpx11.wptType>();

            var number = 1;

            foreach (var profile in profiles)
            {
                waypoints.Add(new gpx11.wptType()
                {
                    lat = (decimal)profile.Track.FirstPoint.Latitude,
                    lon = (decimal)profile.Track.FirstPoint.Longitude,
                    name = "#" + number + " - " + profile.Name
                });

                number++;
            }

            var gpx = new gpx11.gpxType();
            gpx.creator = "Altidude.net";
            gpx.version = "0.1";
            gpx.wpt = waypoints.ToArray();



            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<gpx11.gpxType>(gpx,
                          new System.Net.Http.Formatting.XmlMediaTypeFormatter
                          {
                              UseXmlSerializer = true
                          })
            };
        }

        private gpx11.wptType CreateWaypoint(TrackPoint point)
        {
            return new gpx11.wptType()
            {
                lat = (decimal) point.Latitude,
                lon = (decimal) point.Longitude,
                ele = (decimal) point.Altitude
            };
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/profiles/{profileId}/gpx")]
        public HttpResponseMessage GetProfileGpxFile([FromUri]Guid profileId)
        {
            var views = ApplicationManager.BuildViews();

            var profile = views.Profiles.GetById(profileId);

            var waypoints = profile.Track.Points.Select(CreateWaypoint).ToArray();


            var trkseg = new gpx11.trksegType();
            trkseg.trkpt = waypoints;

            var trk = new gpx11.trkType();
            trk.name = profile.Name;
            trk.trkseg = new[] {trkseg};

            var gpx = new gpx11.gpxType();
            gpx.creator = "Altidude.net";
            gpx.version = "0.1";
            gpx.trk = new gpx11.trkType[] {trk};


            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<gpx11.gpxType>(gpx,
                          new System.Net.Http.Formatting.XmlMediaTypeFormatter
                          {
                              UseXmlSerializer = true
                          })
            };
        }

    }
}
