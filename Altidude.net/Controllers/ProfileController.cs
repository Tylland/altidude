using Altidude.net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Web.Mvc;
using Serilog;
using Altidude.Contracts.Commands;

namespace Altidude.net.Controllers
{
    public class ProfileController : Controller
    {
        private static ILogger _log = Log.ForContext<ProfileController>();

        // GET: Profile
        public ActionResult Index()
        {
            var views = ApplicationManager.BuildViews();

            var viewModel = new ProfileIndexViewModel();

            viewModel.Profiles = views.Profiles.GetAll();

            return View(viewModel);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        private static readonly Guid AdminId = new Guid("d582220a-f65f-42c0-99c6-4c67e64ab9c8");

        [Authorize]
        public ActionResult Edit(Guid id)
        {
            _log.Debug("Edir profile {id}", id);

            var userId = new Guid(User.Identity.GetUserId());
            var views = ApplicationManager.BuildViews();

            var profile = views.Profiles.GetById(id);

            if (profile.UserId != userId && userId != AdminId)
                return RedirectToAction("detail", new { id = id });

            return View(new ProfileEditViewModel() { ProfileId = id });
        }

        [Authorize]
        public ActionResult EditPreview(Guid id)
        {
            _log.Debug("Edir profile {id}", id);

            var userId = new Guid(User.Identity.GetUserId());
            var views = ApplicationManager.BuildViews();

            var profile = views.Profiles.GetById(id);

            if (profile.UserId != userId && userId != AdminId)
                return RedirectToAction("detail", new { id = id });

            return View(new ProfileEditViewModel() { ProfileId = id });
        }

        public ActionResult Detail(Guid id, string source)
        {
            var referrer = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : null;

            if(referrer == null)
                referrer = source != null ? source : "<null>";


            _log.Debug($"Detail page referrer: {referrer}");

            Guid userId = Guid.Empty;

            if(User.Identity.IsAuthenticated)
                userId = new Guid(User.Identity.GetUserId());

            var chartImageUrl = string.Format("https://altidude.blob.core.windows.net/chartimages/{0}.png", id);

            var application = ApplicationManager.BuildApplication();

            var profile = application.Views.Profiles.GetById(id);
            var chartType = application.Views.ChartTypes.GetById(profile.ChartId);

            var chartTypeName = chartType?.Name;
            var athleteDisplayName = profile.Result?.Athlete?.DisplayName;

            application.ExecuteCommand(new RegisterProfileView(profile.Id, referrer));


            var title = chartType != null ? chartType.GetTitle(profile.Result.Athlete.DisplayName, profile.Name) : string.Empty;
            var description = chartType != null ? chartType.GetDescription(profile.Result.Athlete.DisplayName, profile.Name) : string.Empty;

            return View(new ProfileDetailViewModel(id, userId, title, description, chartTypeName, athleteDisplayName, chartImageUrl));
        }

    }
}