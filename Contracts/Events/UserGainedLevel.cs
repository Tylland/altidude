using Altidude.Contracts.Types;
using System;

namespace Altidude.Contracts.Events
{
    public class UserGainedLevel : IEvent
    {
        public Guid Id { get; set; }
        public UserLevel Level { get; set; }

        public UserGainedLevel(Guid id, UserLevel level)
        {
            Id = id;
            Level = level;
        }
        public UserGainedLevel()
        {

        }
    }
}
