using Altidude.Contracts.Types;
using System;

namespace Altidude.Contracts.Events
{
    public class UserUnfollowed: IEvent
    {
        public Guid Id { get; set; }
        public Guid FollowingUserId { get; set; }

        public UserUnfollowed(Guid id, Guid followingUserId)
        {
            Id = id;
            FollowingUserId = followingUserId;
        }
        public UserUnfollowed()
        {

        }
    }
}
