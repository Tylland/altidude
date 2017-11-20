using System;
using System.Collections.Generic;
using Altidude.Contracts;

namespace Altidude.Domain
{
    public interface IDomainRepository
    {
        TResult GetById<TResult>(Guid id) where TResult : IAggregate, new();
        IEnumerable<IEvent> Save<TAggregate>(TAggregate aggregate) where TAggregate : IAggregate;
        void ProcessEvents(IEventBus eventBus, ICheckpointStorage checkpointStorage);
    }
}