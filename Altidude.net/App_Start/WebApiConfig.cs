using Altidude.Infrastructure;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Newtonsoft.Json.Serialization;
using Serilog;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Web.Http;
using ServiceStack.OrmLite;

namespace Altidude.net
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            var setting = CloudConfigurationManager.GetSetting("AltidudeStorageConnectionString");
            var cloudStorageAccount = CloudStorageAccount.Parse(setting);

            Serilog.Debugging.SelfLog.Enable(msg => Debug.WriteLine(msg));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.AzureTableStorage(cloudStorageAccount, storageTableName: "SerilogTable")
                .CreateLogger();


            Log.Debug("Logging Started!!");

            //var connectionString = ConfigurationManager.ConnectionStrings[ApplicationManager.DatabaseConnectionStringName].ConnectionString;
            //var result = DbUpgrader.Upgrade(connectionString, true);

           
            var jsonFormatter = config.Formatters.JsonFormatter;
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            //jsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
