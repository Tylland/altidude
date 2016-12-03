using System;
using Altidude.Contracts.Types;

namespace Altidude.Contracts.Commands
{
    public class DeleteProfile : ICommand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public DeleteProfile(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }

        public DeleteProfile()
        {

        }
    }
}
