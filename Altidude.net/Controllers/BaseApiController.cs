using Altidude.net.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;

namespace Altidude.net.Controllers
{
    public class BaseApiController : ApiController
    {
        private ApplicationUser _member;

        public ApplicationUserManager UserManager
        {
            get { return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        public Guid UserId
        {
            get
            {
                var user = UserManager.FindById(User.Identity.GetUserId()); 

                return user.UserId;
            }
        }

        public ApplicationUser UserRecord
        {
            get
            {
                if (_member != null)
                {
                    return _member;
                }
                _member = UserManager.FindByEmail(Thread.CurrentPrincipal.Identity.Name);
                return _member;
            }
            set { _member = value; }
        }
    }
}