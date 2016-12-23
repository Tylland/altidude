using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using Altidude.Contracts.Types;
using Altidude.net.Models;
using ServiceStack.OrmLite;
using Altidude.Infrastructure;
using System.Configuration;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Altidude.Infrastructure.SqlServerOrmLite;
using gpx = Altidude.Files.Gpx11;
using System.Net.Http;
using System.Text;
using System.Net;
using Altidude.Contracts.Commands;

namespace Altidude.net.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationUserManager _userManager;

        public AdminController(ApplicationUserManager userManager)
        {
            _userManager = userManager; ;
        }

        public AdminController()
        {

        }

        public ApplicationUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        public Guid UserId
        {
            get
            {
                var user = UserManager.FindById(User.Identity.GetUserId());

                return user.UserId;
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            var views = ApplicationManager.BuildViews();

            var model = new AdminViewModel();

            model.TotalNrOfUsers = views.Users.GetAll().Count;
            model.TotalNrOfProfiles = views.Profiles.GetAll().Count;
            model.TotalNrOfPlaces = views.Places.GetAll().Count;

            return View(model);
        }


        public ActionResult Users()
        {
            var views = ApplicationManager.BuildViews();

            var model = new UsersViewModel();

            model.Users = views.Users.GetAll();

            return View(model);
        }

        public ActionResult Profiles()
        {
            var views = ApplicationManager.BuildViews();

            var model = new ProfilesViewModel();

            model.Profiles = views.Profiles.GetSummaries();

            return View(model);
        }

        public ActionResult Import()
        {
            return View();
        }

        private string GenerateCreateScript(params Type[] types)
        {
            string createScript = string.Empty;

           var dialect = new CustomSqlServerOrmLiteDialectProvider();

            //var dialect = SqlServerDialect.Provider;

            OrmLiteConfig.DialectProvider = dialect;

            foreach (var type in types)
            {
                if(createScript != string.Empty)
                    createScript += "\r\n\r\n";

                createScript += dialect.ToCreateTableStatement(type);
            }

            return createScript;
        }
        public ActionResult Database()
        {
            var model = new DatabaseViewModel
            {
                CreateScript = GenerateCreateScript(typeof (PlaceEnvelope), typeof(ProfileEnvelope), typeof(TrackBoundaryView), typeof(UserView))
            };
                       
            return View(model);
        }

        [HttpPost]
        public ActionResult UpgradeDatabase()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[ApplicationManager.DatabaseConnectionStringName].ConnectionString;

            var result = DbUpgrader.Upgrade(connectionString, true);

            return View("Index");
        }

        [HttpPost]
        public ActionResult AdjustUserXP()
        {
            var application = ApplicationManager.BuildApplication();

            var users = application.Views.Users.GetAll();
            var profiles = application.Views.Profiles.GetSummaries();

            foreach(var user in users)
            {
                var xp = 10;

                var userProfiles = profiles.Where(profile => profile.UserId == user.Id);

                xp += userProfiles.Count() * 5;

                xp += userProfiles.Sum(profile => profile.NrOfViews / 10);

                var points = xp - user.ExperiencePoints;

                application.ExecuteCommand(new RegisterUserExperience(user.Id, points));
            }

            return View("Index");
        }



        //[HttpGet]
        //public HttpResponseMessage GetProfilePositionFile()
        //{
        //    var views = ApplicationManager.BuildViews();

        //    var profiles = views.Profiles.GetAll();

        //    var waypoints = new List<gpx.wptType>();

        //    foreach (var profile in profiles)
        //    {
        //        waypoints.Add(new gpx.wptType()
        //        {
        //            lat = (decimal)profile.Track.FirstPoint.Latitude,
        //            lon = (decimal)profile.Track.FirstPoint.Longitude,
        //            name = profile.Name
        //        });
        //    }

        //    var gpx = new gpx.gpxType();
        //    gpx.creator = "Altidude.net";
        //    gpx.version= "0.1";
        //    gpx.wpt = waypoints.ToArray();



        //    return new HttpResponseMessage(HttpStatusCode.OK)
        //    {
        //        Content = new ObjectContent<gpx.gpxType>(gpx,
        //                  new System.Net.Http.Formatting.XmlMediaTypeFormatter
        //                  {
        //                      UseXmlSerializer = true
        //                  })
        //    };
        //}


        [HttpGet]
        public FileContentResult GetUserMailingList()
        {
            var views = ApplicationManager.BuildViews();

            var users = views.Users.GetAll();

            var csv = new StringBuilder();
            csv.AppendLine("Email Addresses,First Name,Last Name");

            foreach (var user in users)
            {
                if(user.AcceptsEmails)
                    csv.AppendLine(user.Email + "," + user.FirstName + "," + user.LastName);
            }

            return File(new UTF8Encoding().GetBytes(csv.ToString()), "text/csv", "altidude.mailinglist.csv");
        }



        [HttpPost]
        public ActionResult Upload()
        {
            if (Request.Files.Count > 0)
            {
                var file = Request.Files[0];

                if (file != null && file.ContentLength > 0)
                {
                    var kmzFile = KmzFile.Open(file.InputStream);
                    var kmlString = kmzFile.ReadKml();

                    Parser parser = new Parser();
                    parser.ParseString(kmlString, false);

                    Kml kml = parser.Root as Kml;
                    var places = new Dictionary<string, Place>();

                    ExtractPlaces(kml.Feature as Feature, "", places);

                    var placeRepository = new OrmLitePlaceRepository(ApplicationManager.OpenConnection());

                    foreach (var place in places.Values)
                    {
                        place.UsageLevel = PlaceUsageLevel.Public;
                        placeRepository.Add(place);
                    }
                }
            }

            return View("Import");
        }


       const string NamespaceSeparator = ".";
       private void ExtractPlaces(Feature feature, string @namespace, Dictionary<string, Place> places)
        {
            // Is the passed in value a Placemark?
            Placemark placemark = feature as Placemark;
            if (placemark != null)
            {
                var name = placemark.Name;
                var fullName = @namespace + NamespaceSeparator + name;
                GeoLocation location = null;
                GeoPolygon fence = null;

                var point = placemark.Geometry as Point;

                if (point != null)
                {
                    location = new GeoLocation(point.Coordinate.Latitude, point.Coordinate.Longitude, (double)point.Coordinate.Altitude);
                    fence = GeoPolygon.CreateRect(location, 0.05);

                    if (!places.ContainsKey(fullName))
                        places.Add(fullName, new Place(Guid.NewGuid(), UserId, placemark.Name, @namespace, location, fence, PlaceAttribute.CreateDefault()));
                    else
                    {
                        var place = places[fullName];
                        place.Location = location;
                    }
                }

                var polygon = placemark.Geometry as Polygon;

                if (polygon != null)
                {
                    var locations = polygon.OuterBoundary.LinearRing.Coordinates.Select(vector => new GeoLocation(vector.Latitude, vector.Longitude, (double)vector.Altitude));

                    fence = new GeoPolygon(locations.ToArray());

                    location = fence.Boudary.GetCenter();

                    if (!places.ContainsKey(fullName))
                    {
                        places.Add(fullName, new Place(Guid.NewGuid(), UserId, placemark.Name, @namespace, location, fence, null));
                    }
                    else
                    {
                        var place = places[fullName];
                        place.Polygon = fence;
                    }
                }
            }
            else
            {
                // Is it a Container, as the Container might have a child Placemark?
                Container container = feature as Container;
                if (container != null)
                {
                    var newNamespace = string.Empty;

                    if (!(container is Document))
                    { 
                        if(@namespace != string.Empty)
                            newNamespace = @namespace + NamespaceSeparator + container.Name;
                        else
                            newNamespace = container.Name;
                    }
                    // Check each Feature to see if it's a Placemark or another Container
                    foreach (var f in container.Features)
                    {
                        ExtractPlaces(f, newNamespace, places);
                    }
                }
            }
        }
    }
}