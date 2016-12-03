using System;

namespace Altidude.Messaging
{
    public interface IEvent
    {
        Guid Id { get; }
    }
}
