using System;

namespace Altidude.Contracts.Commands
{
    public class ClearFollowingUsers : ICommand
    {
        public Guid Id { get; set; }
        public ClearFollowingUsers(Guid id)
        {
            Id = id;
        }
        public ClearFollowingUsers()
        {

        }
    }
}
