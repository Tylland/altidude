using Altidude.Contracts.Types;
using System;

namespace Altidude.Contracts.Events
{
    public class UserFollowed: IEvent
    {
        public Guid Id { get; set; }
        public Guid FollowingUserId { get; set; }

        public UserFollowed(Guid id, Guid followingUserId)
        {
            Id = id;
            FollowingUserId = followingUserId;
        }
        public UserFollowed()
        {

        }
    }
}
