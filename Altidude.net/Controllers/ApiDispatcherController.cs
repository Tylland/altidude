using Altidude.Application;
using Altidude.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using JWT;
using JWT.Serializers;
using Newtonsoft.Json;
using Serilog;

namespace Altidude.net.Controllers
{
    public class ApiDispatcherController : ApiController
    {
        private const string WebJobClientId = "MmL5eJ28RM0mSLYtW1WewIr2skMa8a";
        private const string WebJobSecret = "hjJyOIaXP83ohypJuE3zRh6FWu0fI6";

        private static readonly ILogger Log = Serilog.Log.ForContext<ApiDispatcherController>();

        public class InditityClaim
        {
            public string ClientId { get; set; }
        }

        public static bool IsRunning { get; set; }

        private bool ValidateToken(string token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                IDateTimeProvider provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);

                var json = decoder.Decode(token, WebJobSecret, true);

                Console.WriteLine(json);

                var claim = JsonConvert.DeserializeObject<InditityClaim>(json);

                return claim.ClientId == WebJobClientId;
            }
            catch (TokenExpiredException)
            {
                Console.WriteLine("Token has expired");
            }
            catch (SignatureVerificationException)
            {
                Console.WriteLine("Token has invalid signature");
            }

            return false;
        }

        private bool AuthorizationTokenIsValid()
        {
            var bearerToken = GetHeaderValue("Authorization");

            var token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

            return ValidateToken(token);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/dispatcher/process")]
        public IHttpActionResult ClientProcess()
        {
            Log.Debug("ClientProcess started");
            if (AuthorizationTokenIsValid())
            {
                Log.Debug("Authorization token is valid");
                try
                {
                    ProcessEvents();
                }
                catch (Exception ex)
                {
                    return InternalServerError(ex);
                }

                return Ok();
            }

            return Unauthorized();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("api/dispatcher/process")]
        public IHttpActionResult AdminProcess()
        {
            try
            {
                ProcessEvents();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }

            return Ok();
        }

        private void ProcessEvents()
        {
            Log.Debug("ProcessEvents called");
            if (!IsRunning)
            {
                try
                {
                    IsRunning = true;
                    Log.Debug("ProcessEvents start processing");

                    try
                    {
                    var checkpointStorage = new OrmLiteCheckpointStorage(ApplicationManager.OpenConnection());

                    var application = ApplicationManager.CreateDispatcherWebJob();

                    application.ProcessEvents(checkpointStorage);

                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "ProcessEvents failed"); 
                        throw;
                    }

                    //var domainRepository =
                    //    new NEventStoreDomainRepository(ApplicationManager.DatabaseConnectionStringName);

                    //var eventHandlers = new EventHandlerContainer();
                    //var eventBus = new ApplicationEventBus(eventHandlers);


                    //eventHandlers.Add(
                    //    new SlackMessageSender(
                    //        new Uri("https://hooks.slack.com/services/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")));

                    //domainRepository.ProcessEvents(eventBus, checkpointStorage);
                }
                finally
                {
                    IsRunning = false;
                }
            }
        }


        private string GetHeaderValue(string name)
        {
            IEnumerable<string> values;

            if (Request.Headers.TryGetValues(name, out values))
                return values.FirstOrDefault();

            return string.Empty;
        }

    }
}
