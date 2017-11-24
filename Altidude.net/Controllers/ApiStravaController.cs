using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Claims;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

using Newtonsoft.Json;

using Strava.Authentication;
using Strava.Clients;
using Strava.Activities;
using Strava.Segments;
using Strava.Streams;



using Altidude.net.Models;
using Altidude.Contracts.Commands;
using Altidude.Contracts.Types;
//using Altidude.;
using Newtonsoft.Json.Serialization;
using System.Configuration;
using Altidude.Domain.Aggregates.Profile;
using Serilog;
using Altidude.Infrastructure;

namespace Altidude.net.Controllers
{
    public class ApiStravaController : BaseApiController
    {
        private static ILogger _log = Log.ForContext<ApiStravaController>();

        private const string NoToken = "";
        private const int ClientId = 10139;
        private const string ClientSecret = "bd286a97eeecb97e24ded3fbd71671676350387f";
        private const string StravaAccessTokenClaim = "stravaAccessToken";

        private readonly string _exchangeUri;
        private ApplicationUserManager _userManager;
        public ApiStravaController(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public ApiStravaController()
        {
            _exchangeUri = ConfigurationManager.AppSettings["StravaExchangeUri"];
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/strava/connect")]
        public HttpResponseMessage Connect()
        {
            string url = "https://www.strava.com/oauth/authorize";
//            var exchangeUri = "http://localhost:49367/api/v1/strava/exchange";
            var scopeLevel = "view_private";

            var redirectUri = string.Format("{0}?client_id={1}&response_type=code&redirect_uri={2}&scope={3}&approval_prompt=auto", url, ClientId, _exchangeUri, scopeLevel);

            _log.Debug("Connection Strava with {redirectUri}", redirectUri);

            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri(redirectUri);

            return response;
        }

        public class StravaCodeExcange
        {
            public string State { get; set; }
            public string Code { get; set; }
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/strava/exchange")]
        public async Task<HttpResponseMessage> ExchangeAsync([FromUri]StravaCodeExcange codeExchange)
        {
            _log.Debug("Recieving excenge request from Strava");
            try
            {
                if (codeExchange != null && !string.IsNullOrEmpty(codeExchange.Code))
                {
                    string requestUri = string.Format("https://www.strava.com/oauth/token?client_id={0}&client_secret={1}&code={2}", ClientId, ClientSecret, codeExchange.Code);

                    string json = await Strava.Http.WebRequest.SendPostAsync(new Uri(requestUri));


                    AccessToken accessToken = JsonConvert.DeserializeObject<AccessToken>(json);

                    UpdateStravaAccessTokenClaim(accessToken);
                }
            }
            catch (Exception exception)
            {
                _log.Error(exception, "Recieving excenge failed");
                HttpError err = new HttpError(exception.Message);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, err);
            }

            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri("/profile/create/", UriKind.Relative);

            return response;

            //return null;
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/strava/isconnected")]
        public bool IsConnected()
        {
            return GetClaimedToken() != NoToken;
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/strava/disconnect")]
        public void Disconnect()
        {
            UpdateStravaAccessTokenClaim(null);
        }

        private void UpdateStravaAccessTokenClaim(AccessToken accessToken)
        {
            var userId = User.Identity.GetUserId();

            _log.Debug("Update strava access token claim for {userId}", userId);

            var context = Request.GetOwinContext();
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));

            var claims = manager.GetClaims(userId).ToList();
            var oldClaims = claims.Where(c => c.Type == StravaAccessTokenClaim);

            _log.Debug("Removing nr of old tokens {nrOfOldClaims}", oldClaims.Count());

            foreach (var claim in oldClaims)
                manager.RemoveClaim(userId, claim);

            if (accessToken != null && !string.IsNullOrEmpty(accessToken.Token))
            {
                _log.Debug("Adding token claim for user {userId}, token: {token}", userId, accessToken.Token);

                manager.AddClaim(userId, new Claim(StravaAccessTokenClaim, accessToken.Token));
            }
        }

        private string GetClaimedToken()
        {
            var userId = User.Identity.GetUserId();

            var context = Request.GetOwinContext();
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));

            var userClaims = manager.GetClaims(userId).ToList();
            var tokenClaim = userClaims.FirstOrDefault(c => c.Type == StravaAccessTokenClaim);

            _log.Debug("Get claim token found token {tokenClaim}", tokenClaim);

            if (tokenClaim != default(Claim))
                return tokenClaim.Value;

            return NoToken;
        }

        private StravaClient CreateClient()
        {
            StaticAuthentication auth = new StaticAuthentication(GetClaimedToken());

            return new StravaClient(auth);
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/strava/activities")]
        public List<ActivitySummary> GetActivities(int page, int perPage)
        {
            var client = CreateClient();

            return client.Activities.GetActivities(page, perPage);
        }

        private StravaActivitySummary CreateStravaActivitySummary(ActivitySummary activity)
        {
            try
            {
                return new StravaActivitySummary(activity.Id, activity.Type.ToString(), activity.Name, activity.StartDate, activity.ElapsedTime.ToString(), activity.Distance);
            }
            catch (Exception exception)
            {
                _log.Error(exception, "Failed to create activity: {activityId} '{activityName}'!", activity.Id, activity.Name);
                return null;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/strava/activities/all")]
        public StravaActivitySummary[] GetAllActivities()
        {
            try
            {
                var client = CreateClient();

                var activities = client.Activities.GetAllActivities();

                return activities
                    .Select(activity => CreateStravaActivitySummary(activity))
                    .Where(act => act != null).ToArray();
            }
            catch(Exception exception)
            {
                _log.Error(exception, "GetAllActivities failed!");

                throw exception;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/strava/segments")]
        public List<SegmentSummary> GetSegments()
        {   
            var client = CreateClient();

            var segments = client.Segments.GetStarredSegments();

            return segments;
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/strava/activities/{activityId}/efforts")]
        public List<SegmentEffort> GetActivityEfforts([FromUri]string activityId)
        {
            var client = CreateClient();

            var activity = client.Activities.GetActivity(activityId, true);

            return activity.SegmentEfforts;
        }

        [HttpGet]
        [Authorize]
        [Route("api/v1/strava/activities/{activityId}/profile/import/chart/{chartId}")]
        public Profile ImportProfile([FromUri]string activityId, [FromUri]Guid chartId)
        {
            _log.Debug("Creating profile from strava activity {activityId} med chart {chartId}", activityId, chartId);

            StaticAuthentication auth = new StaticAuthentication(GetClaimedToken());
            StravaClient client = new StravaClient(auth);

            StreamType streamTypes = StreamType.LatLng | StreamType.Distance | StreamType.Altitude | StreamType.Time;

            var activity = client.Activities.GetActivity(activityId, true);
            var streams = client.Streams.GetActivityStream(activityId, streamTypes);

            var id = Guid.NewGuid();
            var track = CreateTrack(activity, streams);

            
            var application = ApplicationManager.BuildApplication();

            application.ExecuteCommand(new CreateProfile(id, UserId, chartId, activity.Name, track));

            return application.Views.Profiles.GetById(id);
        }

        private Track CreateTrack(Activity activity, List<ActivityStream> streams)
        {
            List<TrackPoint> trackPoints = new List<TrackPoint>();

            var latLngJArray = new Newtonsoft.Json.Linq.JArray(
                streams.First(stream => stream.StreamType == StreamType.LatLng).Data.ToArray()
                );

            var latLngList = latLngJArray.ToObject<List<float[]>>();
            

            //foreach(var item in latLngJArray)
            //    item.
            //var latLngList = latLngJArray.SelectMany(token => token.Values<float[]>()).ToArray();


            var distanceArray = streams.First(stream => stream.StreamType == StreamType.Distance).Data.Select(obj => (double)obj).ToArray();
            var altitudeArray = streams.First(stream => stream.StreamType == StreamType.Altitude).Data.Select(obj => (double)obj).ToArray();
            var timeArray = streams.First(stream => stream.StreamType == StreamType.Time).Data.Select(obj => TimeSpan.FromSeconds(Convert.ToDouble(obj))).ToArray();


            for (int i = 0; i < latLngList.Count; i++)
            {
                var latitude = latLngList[i][0];
                var longitude = latLngList[i][1];
                var distance = distanceArray[i];
                var altitude = altitudeArray[i];
                var time = activity.DateTimeStart + timeArray[i];

                var trackpoint = new TrackPoint(latitude, longitude, altitude, distance, time);

                trackPoints.Add(trackpoint);
            }

            var climbs = new ClimbFinder().Find(trackPoints.ToArray());

            return new Track(Guid.NewGuid(), trackPoints.ToArray(), climbs);
        }


        //public static List<Trackpoint> CreateTrack(this gpx11.gpxType gpx)
        //{
        //    var track = new List<Trackpoint>();

        //    var transformer = new GaussKreugerTranformation(Ellipsoid.WGS84);

        //    if (gpx.trk.Length > 0)
        //    {
        //        var trk = gpx.trk[0];

        //        double distance = 0.0;
        //        Coordinate lastCoordinate = null;

        //        foreach (var trkseg in trk.trkseg)
        //        {
        //            foreach (var trkpt in trkseg.trkpt)
        //            {
        //                var coodinate = transformer.ToCoordinate((double)trkpt.lat, (double)trkpt.lon);

        //                if (lastCoordinate != null)
        //                    distance += lastCoordinate.DistanceTo(coodinate);

        //                lastCoordinate = coodinate;

        //                var trackpoint = new Trackpoint((double)trkpt.lat, (double)trkpt.lon, (double)trkpt.ele, distance, trkpt.time);

        //                track.Add(trackpoint);
        //            }
        //        }
        //    }

        //    return track;
        //}

    }
}
