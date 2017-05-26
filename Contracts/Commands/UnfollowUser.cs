using System;

namespace Altidude.Contracts.Commands
{
    public class UnfollowUser : ICommand
    {
        public Guid Id { get; set; }
        public Guid FollowingUserId { get; set; }

        public UnfollowUser(Guid id, Guid followingUserId)
        {
            Id = id;
            FollowingUserId = followingUserId;
        }

        public UnfollowUser()
        {

        }
    }
}
