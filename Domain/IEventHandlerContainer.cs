namespace Altidude.Domain
{
    public interface IEventHandlerContainer
    {
        IEventHandlerContainer Add(object obj);
        IEventHandlerContainer Remove(object obj);

        T[] ResolveAll<T>() where T : class;
    }
}
