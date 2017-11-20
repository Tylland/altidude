using Altidude.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Domain
{
    public interface IEventBus
    {
        void Register<T>(Action<T> callback) where T : IEvent;
        void ClearCallbacks();
        void Raise<T>(T args) where T : IEvent;
        void Raise(IEvent evt);
    }
}
