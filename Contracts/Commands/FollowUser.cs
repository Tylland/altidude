using System;

namespace Altidude.Contracts.Commands
{
    public class FollowUser : ICommand
    {
        public Guid Id { get; set; }
        public Guid FollowingUserId { get; set; }

        public FollowUser(Guid id, Guid followingUserId)
        {
            Id = id;
            FollowingUserId = followingUserId;
        }

        public FollowUser()
        {

        }
    }
}
