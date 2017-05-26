using System;
using System.Configuration;
using System.Data;
using Altidude.Infrastructure;
using Altidude.Infrastructure.SqlServerOrmLite;
using ServiceStack.OrmLite;
using Serilog;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;

namespace MigrationsWebJob
{
    // To learn more about Microsoft Azure WebJobs SDK, please see https://go.microsoft.com/fwlink/?LinkID=320976
    class Program
    {
        // Please set the following connection strings in app.config for this WebJob to run:
        // AzureWebJobsDashboard and AzureWebJobsStorage
        static void Main()
        {
            var setting = CloudConfigurationManager.GetSetting("AltidudeStorageConnectionString");
            var cloudStorageAccount = CloudStorageAccount.Parse(setting);

            Log.Logger = new LoggerConfiguration()
               .MinimumLevel.Verbose()
               .WriteTo.AzureTableStorage(cloudStorageAccount, storageTableName: "WebJobs")
               .CreateLogger();


            Log.Debug("Migrations Web Job Started!!");

            RunMigration();
        }

        public static IDbConnection OpenConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["AltidudeConnection"].ConnectionString;

            var dialect = new CustomSqlServerOrmLiteDialectProvider();
            OrmLiteConfig.DialectProvider = dialect;

            var dbFactory = new OrmLiteConnectionFactory(connectionString, dialect);

            return dbFactory.Open();
        }
        private static void RunMigration()
        {
            try
            {
                var migrationManager = new MigrationManager(OpenConnection());
                migrationManager.Apply();
            }
            catch (Exception e)
            {
                Log.Logger.Error(e, "RunMigration failed!");
                throw;
            }
        }
    }
}
