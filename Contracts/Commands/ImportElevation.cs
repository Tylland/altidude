using System;

namespace Altidude.Contracts.Commands
{
    public class ImportElevation : ICommand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public ImportElevation(Guid id, Guid userId)
        {
            Id = id;
            UserId = userId;
        }

        public ImportElevation()
        {

        }
    }
}
