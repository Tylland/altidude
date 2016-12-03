using Altidude.net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Altidude.net.Controllers
{
    public class FacebookController : Controller
    {
        // GET: Facebook
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ShareProfile(Guid id)
        {
            return View(new FacebookShareProfileViewModel(id));
        }
        public ActionResult SharedProfile()
        {
            return View();
        }
    }
}