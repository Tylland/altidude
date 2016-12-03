using Altidude.Contracts;

namespace Altidude.Domain
{
    public interface IHandle<in TCommand> where TCommand : ICommand
    {
        IAggregate Handle(TCommand command);
    }
}
