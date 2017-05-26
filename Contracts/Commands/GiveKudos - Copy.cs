using System;

namespace Altidude.Contracts.Commands
{
    public class GiveKudos : ICommand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public GiveKudos(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }

        public GiveKudos()
        {

        }
    }
}
