using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Transactions;
using System.Threading.Tasks;
using Altidude.Contracts;
using Altidude.Domain;
using NEventStore;
using NEventStore.Dispatcher;
using NEventStore.Persistence.Sql;
using NEventStore.Persistence.Sql.SqlDialects;
using Serilog;
using Serilog.Core;

namespace Altidude.Infrastructure
{
    public class NEventStoreDomainRepository : DomainRepositoryBase
    {
        private static readonly ILogger Log = Serilog.Log.ForContext<NEventStoreDomainRepository>();

        private readonly IStoreEvents _store;

        public NEventStoreDomainRepository(string connectionStringName)
        {
            _store = Wireup.Init()
             .UsingSqlPersistence(connectionStringName)
             .WithDialect(new MsSqlDialect())
            .InitializeStorageEngine()
            .UsingJsonSerialization()
            .Compress()
            .Build();
        }

        public override TResult GetById<TResult>(Guid id) 
        {
            using (var stream = _store.OpenStream(id, 0, int.MaxValue))
            {
                var commitedEvents = stream.CommittedEvents.Select(em => em.Body as IEvent);

                return BuildAggregate<TResult>(commitedEvents);
            }
        }

        public override IEnumerable<IEvent> Save<TAggregate>(TAggregate aggregate) 
        {
            var savedEvents = new List<IEvent>();

            int minRevision = aggregate.CommittedVersion < 0 ? 0 : int.MinValue;

            //using (var t = new TransactionScope())
            //{
            
                using (var stream = _store.OpenStream(aggregate.Id, minRevision, int.MinValue))
                {
                    foreach (var uncommitedEvent in aggregate.UncommittedEvents())
                    {
                        stream.Add(new EventMessage {Body = uncommitedEvent});
                        savedEvents.Add(uncommitedEvent);
                    }

                    stream.CommitChanges(Guid.NewGuid());
                }

            //    t.Complete();
            //}

            return savedEvents;
        }

        public IEnumerable<IEvent> GetAllEvents(string checkpointToken = null)
        {
            var commits = _store.Advanced.GetFrom();

            foreach (var commit in commits)
            {
                foreach (var evt in commit.Events)
                {
                    yield return evt.Body as IEvent;
                }
            }
        }

        private int _isProcessing;

        private const string DispatcherIsRunningName = "DispatcherIsRunning";
        private const string DispatcherCheckpointTokenName = "Dispatcher";
        public override void ProcessEvents(IEventBus eventBus, ICheckpointStorage checkpointStorage)
        {
            if (Interlocked.CompareExchange(ref _isProcessing, 1, 0) == 0)
            {
                try
                {
                    var checkpointToken = checkpointStorage?.GetToken(DispatcherCheckpointTokenName);
                    var commits = _store.Advanced.GetFrom(checkpointToken).ToArray();

                    Log.Debug("{NrOfCommits} commits found for checkpoint {CheckpointToken}-{CheckpointName}",
                        commits.Length, checkpointToken ?? "null", DispatcherCheckpointTokenName);

                    foreach (var commit in commits)
                    {
                        foreach (var evt in commit.Events)
                        {
                            try
                            {
                                eventBus.Raise(evt.Body as IEvent);
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex, "Dispatching {@commit} failed", commit);
                                throw;
                            }
                        }
                        checkpointStorage.Save(DispatcherCheckpointTokenName, commit.CheckpointToken);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "ProcessEvents failed");
                }

                Interlocked.Exchange(ref _isProcessing, 0);
            }
        }
    }
}
