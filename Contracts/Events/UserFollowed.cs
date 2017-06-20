using Altidude.Contracts.Types;
using System;

namespace Altidude.Contracts.Events
{
    public class UserFollowed: IEvent
    {
        public Guid Id { get; set; }
        public Guid OtherUserId { get; set; }
        public UserFollowed(Guid id, Guid otherUserId)
        {
            Id = id;
            OtherUserId = otherUserId;
        }
        public UserFollowed()
        {

        }
    }
}
