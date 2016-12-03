using System;

using Altidude.Contracts.Types;
using Altidude.Contracts;

namespace Altidude.Contracts.Events
{
    public class ProfileDeleted : IEvent
    {
        public Guid Id { get; set; }
              
        public ProfileDeleted(Guid id)
        {
            Id = id;
        }
        public ProfileDeleted()
        {

        }
    }
}
