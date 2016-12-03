using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Altidude.net.Controllers
{
    public class RedirectController : ApiController
    {
        [AcceptVerbs("Get", "Post")]
        [Route("api/v1/profiles/import/trackfile")]
        public HttpResponseMessage GetRedirectApi()
        {
            var remoteHost = "http://localhost:30855/";

            var redirectUrl = remoteHost + "api/v1/profiles/import/trackfile";// + requestedUrl;

            var response = Request.CreateResponse(HttpStatusCode.Moved);
            response.Headers.Location = new Uri(redirectUrl);

            return response;
        }
    }
}
