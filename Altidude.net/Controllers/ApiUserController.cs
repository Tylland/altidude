using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Altidude.Contracts.Types;

namespace Altidude.net.Controllers
{
    public class ApiUserController : BaseApiController
    {
        [HttpGet]
        [Route("api/v1/users/{userId}/follow")]
        public void Follow([FromUri]Guid profileId)
        {

        }

    }
}