using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Altidude.Contracts.Types;

namespace Altidude.Contracts.Commands
{
    public class CreateProfile : ICommand
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public Track Track { get; set; }

        public CreateProfile(Guid id, Guid userId, string name, Track track)
        {
            Id = id;
            UserId = userId;
            Name = name;
            Track = track;
        }

        public CreateProfile()
        {

        }
    }
}
