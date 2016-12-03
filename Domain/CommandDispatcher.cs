using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Altidude.Contracts;

namespace Altidude.Domain
{
    public class CommandDispatcher
    {
        private Dictionary<Type, Func<object, IAggregate>> _routes;
        private IDomainRepository _domainRepository;
        private readonly IEnumerable<Action<object>> _postExecutionPipe;
        private readonly IEnumerable<Action<ICommand>> _preExecutionPipe;
        private readonly IEventBus _eventBus; 

        public CommandDispatcher(IDomainRepository domainRepository, IEventBus eventBus, IEnumerable<Action<ICommand>> preExecutionPipe, IEnumerable<Action<object>> postExecutionPipe)
        {
            _domainRepository = domainRepository;
            _eventBus = eventBus;
            _postExecutionPipe = postExecutionPipe;
            _preExecutionPipe = preExecutionPipe ?? Enumerable.Empty<Action<ICommand>>();
            _routes = new Dictionary<Type, Func<object, IAggregate>>();
        }

        public void RegisterHandler<TCommand>(IHandle<TCommand> handler) where TCommand : class, ICommand
        {
            _routes.Add(typeof(TCommand), command => handler.Handle(command as TCommand));
        }

        public void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            var commandType = command.GetType();

            RunPreExecutionPipe(command);
            if (!_routes.ContainsKey(commandType))
            {
                throw new ApplicationException("Missing handler for " + commandType.Name);
            }

            IEnumerable<IEvent> savedEvents;

            try
            {
                KeyMonitor.Enter(command.Id);

                var aggregate = _routes[commandType](command);
                savedEvents = _domainRepository.Save(aggregate);
            }
            finally
            {
                KeyMonitor.Exit(command.Id);
            }

            Raise(savedEvents);
            RunPostExecutionPipe(savedEvents);
        }

        private void Raise(IEnumerable<IEvent> savedEvents)
        {
            if (_eventBus != null)
            {
                foreach (var savedEvent in savedEvents)
                    _eventBus.Raise((dynamic)savedEvent);
            }
        }

        private void RunPostExecutionPipe(IEnumerable<object> savedEvents)
        {
            foreach (var savedEvent in savedEvents)
            {
                foreach (var action in _postExecutionPipe)
                {
                    action(savedEvent);
                }
            }
        }

        private void RunPreExecutionPipe(ICommand command)
        {
            foreach (var action in _preExecutionPipe)
            {
                action(command);
            }
        }
    }
}
