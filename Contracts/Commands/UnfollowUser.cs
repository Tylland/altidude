using System;

namespace Altidude.Contracts.Commands
{
    public class UnfollowUser : ICommand
    {
        public Guid Id { get; set; }
        public Guid OtherUserId { get; set; }

        public UnfollowUser(Guid id, Guid otherUserId)
        {
            Id = id;
            OtherUserId = otherUserId;
        }

        public UnfollowUser()
        {

        }
    }
}
