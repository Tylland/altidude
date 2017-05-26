using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Altidude.Contracts.Commands;
using Altidude.Logging;
using Microsoft.AspNet.Identity;

using Altidude.net.Models;
using Serilog;

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
        public ActionResult Dashboard()
        {
            var views = ApplicationManager.BuildViews();

            var userIds = new List<Guid>();

            var user = views.Users.GetById(UserId);

            userIds.Add(user.Id);
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
        public ActionResult ToggleFollow(Guid userId)
        {
            var followingUserId = new Guid(User.Identity.GetUserId());

            var application = ApplicationManager.BuildApplication();
            var user = application.Views.Users.GetById(userId);

            if(user.IsFollowedBy(followingUserId))
                application.ExecuteCommand(new UnfollowUser(userId, followingUserId));
            else
                application.ExecuteCommand(new FollowUser(userId, followingUserId));

            return RedirectToAction("Index", new { userId = userId });
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