using Altidude.Contracts.Types;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Altidude.Contracts.Commands;
using Altidude.Files;
using tcx20 = Altidude.Files.Tcx20;
using gpx11 = Altidude.Files.Gpx11;
using Altidude.Transformation;

namespace Altidude.net.Controllers
{
    public class ApiChartTypeController : BaseApiController
    {
        [HttpGet]
        [Route("api/v1/charttypes/user")]
        public UserChartType[] GetUserChartTypes()
        {
            var views = ApplicationManager.BuildViews();

            var user = views.Users.GetById(UserId);

            return views.ChartTypes.GetForUser(user.Level, DateTime.Now);
        }

    }
}
