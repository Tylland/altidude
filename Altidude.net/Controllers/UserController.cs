using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Altidude.Contracts.Commands;
using Altidude.Logging;
using Microsoft.AspNet.Identity;

using Altidude.net.Models;
using Serilog;
using ServiceStack.Text;

namespace Altidude.net.Controllers
{
    public class UserController : BaseController
    {
        private static ILogger _log = Log.ForContext<ProfileController>();

        // GET: User
        [Authorize]
        public ActionResult Index(Guid id)
        {
            var visitingUserId = new Guid(User.Identity.GetUserId());

            var views = ApplicationManager.BuildViews();

            var user = views.Users.GetById(id);

            var level = views.Levels.GetLevel(user.Level);
            var isFollowedBy = user.IsFollowedBy(visitingUserId);

            var model = new UserProfileViewModel()
            {
                User = user,
                Level = level,
                LevelProgress = level.GetProgress(user.ExperiencePoints),
                LevelPoints = level.GetLevelPoints(user.ExperiencePoints),
                IsFollowedBy = isFollowedBy,
                FollowingText = isFollowedBy ? "Unfollow" : "Follow"
            };

            return View(model);
        }

        [Authorize]
        public ActionResult Dashboard(Guid? id)
        {
            var ownerId = id != null && id != Guid.Empty && User.IsInRole("Admin") ? id.Value : UserId;

            var views = ApplicationManager.BuildViews();

            var user = views.Users.GetById(ownerId);

            var userIds = new List<Guid> {user.Id};
            userIds.AddRange(user.FollowingUserIds);

            var model = new UserDashboardViewModel()
            {
                ProfileSummary = user.ProfileSummary,
                Profiles = views.Profiles.GetLatestSummaries(userIds.ToArray(), 0, 20)
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public ActionResult ToggleFollow(Guid otherUserId)
        {
            var userId = new Guid(User.Identity.GetUserId());

            var application = ApplicationManager.BuildApplication();
            var user = application.Views.Users.GetById(userId);

            if(user.IsFollowing(otherUserId))
                application.ExecuteCommand(new UnfollowUser(userId, otherUserId));
            else
                application.ExecuteCommand(new FollowUser(userId, otherUserId));

            return RedirectToAction("Index", "User", new { id = otherUserId });
        }

        [Authorize]
        public ActionResult RegisterAsPreviewUser(Guid? id)
        {
            var userId = id != null && id != Guid.Empty && User.IsInRole("Admin") ? id.Value : UserId;

            var user = UserManager.FindById(userId.ToString());

            if (user != null)
            {
                var result1 = UserManager.AddToRole(user.Id, "PreviewUser");

                return RedirectToAction("ShowMessage", "Home", result1.Succeeded ? new { message = "You are now registered as a preview user" } : new { message = "Register as a preview user failed!" + result1.Errors.Join(", ") });
            }

            return RedirectToAction("ShowMessage", "Home", new { message = "Register as a preview user failed!" });
        }

        public PartialViewResult UserLoginPartial()
        {
            using (_log.StartTiming("UserLoginPartial"))
            {
                var views = ApplicationManager.BuildViews();

                var viewModel = new LoginUserViewModel();

                Guid userId = Guid.Empty;

                if (User.Identity.IsAuthenticated)
                {
                    userId = new Guid(User.Identity.GetUserId());

                    viewModel.User = views.Users.GetById(userId);
                }

                return PartialView("~/Views/Shared/_UserLoginPartial.cshtml", viewModel);
            }
        }
    }
}