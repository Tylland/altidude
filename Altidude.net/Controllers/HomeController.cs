using Altidude.Infrastructure;
using Altidude.net.Models;
using Altidude.Infrastructure.SqlServerOrmLite;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using Altidude.Logging;
using Serilog;

namespace Altidude.net.Controllers
{
    public class HomeController : BaseController
    {
        private static ILogger _log = Log.ForContext<HomeController>();

        public ActionResult Index()
        {
            using (_log.StartTiming(false, "HomeController - Load Index Page").WithWarning(10000))
            {
                var model = new HomeViewModel();

                OrmLiteProfileView view;
                using (_log.StartTiming(false, "HomeController - Index Create View").WithWarning(5000))
                {
                    var connectionString =
                        ConfigurationManager.ConnectionStrings[ApplicationManager.DatabaseConnectionStringName]
                            .ConnectionString;

                    var dialect = new CustomSqlServerOrmLiteDialectProvider();
                    OrmLiteConfig.DialectProvider = dialect;

                    var dbFactory = new OrmLiteConnectionFactory(connectionString, dialect);

                    var db = dbFactory.Open();

                    view = new OrmLiteProfileView(db);
                }

                using (_log.StartTiming(false, "HomeController - Index View GetLatest").WithWarning(5000))
                {
                    model.Profiles = view.GetLatestSummaries(21);
                }

                return View(model);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ShowMessage(string message)
        {
            ViewBag.Message = message;

            return View();
        }

        [HttpPost]
        public ActionResult SendContactMessage(ContactUsMessageModel model)
        {
            //MailMessage message = new MailMessage(model.Email, "altidude.net@gmail.com");
            //message.ReplyToList.Add(model.Email);
            //message.Subject = "Contact message";
            //message.Body = model.Message;

            //new GoogleMailSender().Send(message);
            new SendGridMailSender().SendMessage(model.Email, model.Name, "altidude.net@gmail.com", "Altidude", "Contact message", model.Message, null);

            return View("Index");
        }
    }
}