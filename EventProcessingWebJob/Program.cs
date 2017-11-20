using System;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.Azure;
using Microsoft.Azure.WebJobs;
using Microsoft.WindowsAzure.Storage;
using Serilog;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;

namespace Altidude.DispatcherWebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        private const string WebJobClientId = "MmL5eJ28RM0mSLYtW1WewIr2skMa8a";
        private const string WebJobSecret = "hjJyOIaXP83ohypJuE3zRh6FWu0fI6";
        private const string DispatcherUrlKey = "DispatcherUrl";

        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var config = new JobHostConfiguration();

            if (config.IsDevelopment)
            {
                config.UseDevelopmentSettings();
            }

            var setting = CloudConfigurationManager.GetSetting("AltidudeStorageConnectionString");
            var cloudStorageAccount = CloudStorageAccount.Parse(setting);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.AzureTableStorage(cloudStorageAccount, storageTableName: "WebJobs")
                .CreateLogger();


            Log.Debug("Dispatcher Web Job Running!!");

            //var host = new JobHost();


            CallDispatcher();
            //var application = ApplicationManager.BuildApplication();

            //application.DomainEntry.ProcessUndispatchedEvents();

            // The following code ensures that the WebJob will be running continuously
            
            
            
            //host.RunAndBlock();

        }

        private static string CreateToken(Dictionary<string, object> payload, string secret)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var token = encoder.Encode(payload, secret);

            Console.WriteLine(token);

            return token;
        }

        private static void CallDispatcher()
        {
            try
            {
                var dispatcherUrl = ConfigurationManager.AppSettings[DispatcherUrlKey];

                var payload = new Dictionary<string, object> {{"ClientId", WebJobClientId}};

                var token = CreateToken(payload, WebJobSecret);

                Log.Debug("Calling dispatcher at {dispatcherUrl} with payload {payload} and token {token}",
                    dispatcherUrl, payload, token);

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    var response = httpClient.PostAsync(dispatcherUrl, null).Result;

                    var responseContent = response.Content.ReadAsStringAsync();

                    Log.Debug(responseContent.Result);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Calling dispatcher failed");
            }
        }
    }
}
