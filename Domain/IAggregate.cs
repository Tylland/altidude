using System;
using System.Collections.Generic;
using Altidude.Contracts;

namespace Altidude.Domain
{
    public interface IAggregate
    {
        Guid Id { get; }
        int Version { get; }
        int CommittedVersion { get; }
        void ApplyEvent(IEvent @event);
        void ClearUncommittedEvents();
        void RaiseEvent(IEvent @event);
        IEnumerable<IEvent> UncommittedEvents();
    }
}