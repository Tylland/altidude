using System;

namespace Altidude.Contracts.Commands
{
    public class FollowUser : ICommand
    {
        public Guid Id { get; set; }
        public Guid OtherUserId { get; set; }

        public FollowUser(Guid id, Guid followingUserId)
        {
            Id = id;
            OtherUserId = followingUserId;
        }

        public FollowUser()
        {

        }
    }
}
