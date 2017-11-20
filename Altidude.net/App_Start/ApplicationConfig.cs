using System;
using System.Configuration;
using System.Data;
using Altidude.Application;
using Altidude.Domain;
using Altidude.Domain.EventHandlers;
using Altidude.Infrastructure;
using Altidude.Infrastructure.SqlServerOrmLite;
using ServiceStack.OrmLite;

namespace Altidude.net
{
    public class ApplicationManager
    {
        public const string DatabaseConnectionStringName = "AltidudeConnection";
        public const string StorageConnectionStringName = "AltidudeStorageConnectionString";
        public const string SlackSendMessageUriKey = "SlackSendMessageUri";


        public static IDbConnection OpenConnection()
        {
            var connectionString = ConfigurationManager.ConnectionStrings[DatabaseConnectionStringName].ConnectionString;

            var dialect = new CustomSqlServerOrmLiteDialectProvider();
            OrmLiteConfig.DialectProvider = dialect;

            var dbFactory = new OrmLiteConnectionFactory(connectionString, dialect);

            return dbFactory.Open();
        }


        public static ApplicationViews BuildViews()
        {
            var db = OpenConnection();

            var views = new ApplicationViews(new OrmLiteUserView(db), new InMemoryUserLevelService(), new OrmLiteProfileView(db), new OrmLitePlaceRepository(db), new InMemoryChartTypeView());

            return views;
        }

        public static ApplicationInstance BuildApplication()
        {
            var domainRepository = new NEventStoreDomainRepository(DatabaseConnectionStringName);

            var db = OpenConnection();

            var views = new ApplicationViews(new OrmLiteUserView(db), new InMemoryUserLevelService(), new OrmLiteProfileView(db), new OrmLitePlaceRepository(db), new InMemoryChartTypeView());

            var eventHandlers = new EventHandlerContainer();
            eventHandlers.Add(views.Users);
            eventHandlers.Add(views.Profiles);

            var storageConnectionString = ConfigurationManager.AppSettings[StorageConnectionStringName];

            eventHandlers.Add(new AzureBlobChartImageManager(storageConnectionString));

            var dateTimeProvider = new SystemDateTimeProvider();
            var userService = new OrmLiteUserView(db);
            var placeFinder = new OrmLitePlaceRepository(db);
            var elevationService = new GoogleMapsElevationService();

            var domainEntry = new DomainEntry(domainRepository, new ApplicationEventBus(eventHandlers), dateTimeProvider, userService, new InMemoryUserLevelService(), placeFinder, elevationService);

            //eventHandlers.Add(new UserProgressManager(domainEntry));
            //eventHandlers.Add(new SendGridEmailNotifier(userService));
            //eventHandlers.Add(new SlackMessageSender(new Uri("https://hooks.slack.com/services/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")));

            return new ApplicationInstance(domainEntry, views);
        }

        public static ApplicationInstance CreateDispatcherWebJob()
        {
            var domainRepository = new NEventStoreDomainRepository(DatabaseConnectionStringName);

            var db = OpenConnection();

            var views = new ApplicationViews(new OrmLiteUserView(db), new InMemoryUserLevelService(), new OrmLiteProfileView(db), new OrmLitePlaceRepository(db), new InMemoryChartTypeView());

            var eventHandlers = new EventHandlerContainer();

            var dateTimeProvider = new SystemDateTimeProvider();
            var userService = new OrmLiteUserView(db);
            var placeFinder = new OrmLitePlaceRepository(db);
            var elevationService = new GoogleMapsElevationService();

            var domainEntry = new DomainEntry(domainRepository, new ApplicationEventBus(eventHandlers), dateTimeProvider, userService, new InMemoryUserLevelService(), placeFinder, elevationService);

            var slackSendMessageUriKey = ConfigurationManager.AppSettings[SlackSendMessageUriKey];

            eventHandlers.Add(new UserProgressManager(domainEntry));
            eventHandlers.Add(new SendGridEmailNotifier(userService));
            eventHandlers.Add(new OrmLiteTrackBoundaryView(db));
            eventHandlers.Add(new SlackMessageSender(views, new Uri(slackSendMessageUriKey)));

            return new ApplicationInstance(domainEntry, views);
        }
    }

}