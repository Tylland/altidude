using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Domain
{
    public class EventDispatcher
    {
        private readonly IDomainRepository _domainRepository;
        private readonly IEventBus _eventBus;

        public EventDispatcher(IDomainRepository domainRepository, IEventBus eventBus)
        {
            _domainRepository = domainRepository;
            _eventBus = eventBus;
        }

        public void ProcessEvents(ICheckpointStorage checkpointStorage)
        {
            _domainRepository.ProcessEvents(_eventBus, checkpointStorage);
        }
    }
}
