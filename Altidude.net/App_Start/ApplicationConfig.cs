using Altidude.Contracts;
using Altidude.Contracts.Types;
using Altidude.Domain;
using Altidude.Infrastructure;
using Altidude.Views;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using Altidude.Domain.EventHandlers;
using Altidude.Infrastructure.SqlServerOrmLite;
using System.Data;
using System.Threading.Tasks;

namespace Altidude.net
{
    public class ApplicationManager
    {
        public const string DatabaseConnectionStringName = "AltidudeConnection";
        public const string StorageConnectionStringName = "AltidudeStorageConnectionString";

        public class ApplicationInstance 
        {
            public DomainEntry DomainEntry { get; private set; }
            public ApplicationViews Views { get; private set; }
            public ApplicationInstance(DomainEntry domainEntry, ApplicationViews views)
            {
                DomainEntry = domainEntry;
                Views = views;
            }

            public void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
            {
                DomainEntry.ExecuteCommand(command);
            }
            public Task ExecuteCommandAsync<TCommand>(TCommand command) where TCommand : ICommand
            {
                return Task.Run(() => DomainEntry.ExecuteCommand(command));
            }
        }

        public class ApplicationViews
        {
            public IUserView Users { get; private set; }
            public IUserLevelView Levels { get; private set; }
            public IProfileView Profiles { get; private set; }
            public IPlaceView Places { get; private set; }
            public IChartTypeView ChartTypes { get; private set; }

            public ApplicationViews(IUserView users, IUserLevelView levels, IProfileView profiles, IPlaceView places, IChartTypeView chartTypes)
            {
                Users = users;
                Levels = levels;
                Profiles = profiles;
                Places = places;
                ChartTypes = chartTypes;
            }
        }

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

            eventHandlers.Add(new OrmLiteTrackBoundaryView(db));

            var storageConnectionString = ConfigurationManager.AppSettings[StorageConnectionStringName];

            eventHandlers.Add(new AzureBlobChartImageManager(storageConnectionString));

            var dateTimeProvider = new SystemDateTimeProvider();
            var userService = new OrmLiteUserView(db);
            var placeFinder = new OrmLitePlaceRepository(db);
            var elevationService = new GoogleMapsElevationService();

            var domainEntry = new DomainEntry(domainRepository, new ApplicationEventBus(eventHandlers), dateTimeProvider, userService, new InMemoryUserLevelService(), placeFinder, elevationService);

            eventHandlers.Add(new UserProgressManager(domainEntry));
            eventHandlers.Add(new SendGridEmailNotifier(userService));

            return new ApplicationInstance(domainEntry, views);
        }


        public class ApplicationEventBus : IEventBus
        {
            private List<Delegate> _actions;

            public static IEventHandlerContainer Handlers { get; set; } 

            //Registers a callback for the given domain event
            public void Register<T>(Action<T> callback) where T : IEvent
            {
                if (_actions == null)
                    _actions = new List<Delegate>();
                _actions.Add(callback);
            }

            //Clears callbacks passed to Register on the current thread   
            public void ClearCallbacks()
            {
                _actions = null;
            }

            //Raises the given domain event   
            public void Raise<T>(T args) where T : IEvent
            {
                if (Handlers != null)
                {
                    foreach (var handler in Handlers.ResolveAll<IHandleEvent<T>>())
                    {
                        try
                        { 
                            handler.Handle(args);
                        }
                        catch (Exception ex)
                        {
                            //TODO: Logging
                            Console.WriteLine(ex);
                        }
                    }
                }

                if (_actions != null)
                    foreach (var action in _actions)
                        if (action is Action<T>)
                            ((Action<T>)action)(args);
            }

            public ApplicationEventBus(IEventHandlerContainer eventHandlerContainer)
            {
                Handlers = eventHandlerContainer;
            }

        }
    }

    public class EventHandlerContainer : IEventHandlerContainer
    {
        //private class HandlerEntry
        //{
        //    public object Handler { get; private set; }
        //    public bool Synchronized { get; private set; }

        //    public void Handle<T>(T args) where T : class
        //    {
        //        var handler = Handler as T;

        //        if (handler != null)
        //        {
        //            hand
        //        }
        //    }

        //    public HandlerEntry(object handler)
        //        : this(handler, true)
        //    {
        //    }
        //    public HandlerEntry(object handler, bool synchronized)
        //    {
        //        Handler = handler;
        //        Synchronized = synchronized;
        //    }
        //}

        private readonly List<object> _handlers = new List<object>();

        public T[] ResolveAll<T>() where T : class
        {
            var resolved = new List<T>();

            foreach (var obj in _handlers)
            {
                var t = obj as T;

                if (t != null)
                    resolved.Add(t);
            }

            return resolved.ToArray();
        }

        public IEventHandlerContainer Add(object obj)
        {
            _handlers.Add(obj);

            return this;
        }

        public IEventHandlerContainer Remove(object obj)
        {
            _handlers.Remove(obj);

            return this;
        }


    }
}