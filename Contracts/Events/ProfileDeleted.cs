using System;

using Altidude.Contracts.Types;
using Altidude.Contracts;

namespace Altidude.Contracts.Events
{
    public class ProfileDeleted : IEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public ProfileDeleted(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }
        public ProfileDeleted()
        {

        }
    }
}
