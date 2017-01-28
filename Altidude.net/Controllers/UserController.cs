using System;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

using Altidude.net.Models;

namespace Altidude.net.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult UserLoginPartial()
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