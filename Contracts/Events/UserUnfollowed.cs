using Altidude.Contracts.Types;
using System;

namespace Altidude.Contracts.Events
{
    public class UserUnfollowed: IEvent
    {
        public Guid Id { get; set; }
        public Guid OtherUserId { get; set; }
        public UserUnfollowed(Guid id, Guid otherUserId)
        {
            Id = id;
            OtherUserId = otherUserId;
        }
        public UserUnfollowed()
        {

        }
    }
}
