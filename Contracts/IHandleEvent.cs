namespace Altidude.Contracts
{
    public interface IHandleEvent<in TEvent> where TEvent : IEvent
    {
        void Handle(TEvent evt);
    }
}
 