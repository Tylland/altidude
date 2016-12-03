using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Threading.Tasks;
using Altidude.Contracts;
using Altidude.Domain;
using NEventStore;
using NEventStore.Persistence.Sql.SqlDialects;

namespace Altidude.Infrastructure
{
    public class NEventStoreDomainRepository : DomainRepositoryBase
    {
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
    }
}
