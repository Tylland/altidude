using System;

namespace Altidude.Contracts.Events
{
    public class FollowingUsersCleared: IEvent
    {
        public Guid Id { get; set; }
        public FollowingUsersCleared(Guid id)
        {
            Id = id;
        }
        public FollowingUsersCleared()
        {

        }
    }
}
