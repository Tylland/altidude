using System.Threading.Tasks;
using Altidude.Domain;
using Altidude.Contracts;

namespace Altidude.Application
{
    public class ApplicationInstance
    {
        public DomainEntry DomainEntry { get; private set; }
        public ApplicationViews Views { get; private set; }
        public ApplicationInstance(DomainEntry domainEntry, ApplicationViews views)
        {
            DomainEntry = domainEntry;
            Views = views;
        }

        public void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            DomainEntry.ExecuteCommand(command);
        }
        public Task ExecuteCommandAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            return Task.Run(() => DomainEntry.ExecuteCommand(command));
        }

        public void ProcessEvents(ICheckpointStorage checkpointStorage)
        {
            DomainEntry.ProcessEvents(checkpointStorage);
        }
    }
}
