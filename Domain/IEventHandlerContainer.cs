using System.Collections.Generic;

namespace Altidude.Domain
{
    public interface IEventHandlerContainer: IEnumerable<object>
    {
        IEventHandlerContainer Add(object obj);
        IEventHandlerContainer Remove(object obj);

        T[] ResolveAll<T>() where T : class;
    }
}
