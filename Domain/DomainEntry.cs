using Altidude.Contracts;
using Altidude.Contracts.Commands;
using Altidude.Domain.Aggregates.Profile;
using Altidude.Domain.CommandHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Domain
{
    public class DomainEntry
    {
        private readonly CommandDispatcher _commandDispatcher;

        public DomainEntry(IDomainRepository domainRepository, IEventBus eventBus, IDateTimeProvider dateTimeProvider, IUserService userService, IUserLevelService levelService, IPlaceFinder placeFinder, IElevationService elevationService, IEnumerable<Action<ICommand>> preExecutionPipe = null, IEnumerable<Action<object>> postExecutionPipe = null)
        {
            preExecutionPipe = preExecutionPipe ?? Enumerable.Empty<Action<ICommand>>();
            postExecutionPipe = CreatePostExecutionPipe(postExecutionPipe);

            _commandDispatcher = CreateCommandDispatcher(domainRepository, eventBus, dateTimeProvider, userService, levelService, placeFinder, elevationService, preExecutionPipe, postExecutionPipe);
        }

        public void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            _commandDispatcher.ExecuteCommand(command);
        }

        private CommandDispatcher CreateCommandDispatcher(IDomainRepository domainRepository, IEventBus eventBus, IDateTimeProvider dateTimeProvider, IUserService userService, IUserLevelService levelService, IPlaceFinder placeFinder, IElevationService elevationService, IEnumerable<Action<ICommand>> preExecutionPipe, IEnumerable<Action<object>> postExecutionPipe)
        {
            var commandDispatcher = new CommandDispatcher(domainRepository, eventBus, preExecutionPipe, postExecutionPipe);

            var userCommandHandler = new UserCommandHandler(domainRepository, dateTimeProvider, levelService);
            commandDispatcher.RegisterHandler<CreateUser>(userCommandHandler);
            commandDispatcher.RegisterHandler<RegisterUserExperience>(userCommandHandler);
            commandDispatcher.RegisterHandler<UpdateUserSettings>(userCommandHandler);
            commandDispatcher.RegisterHandler<FollowUser>(userCommandHandler);
            commandDispatcher.RegisterHandler<UnfollowUser>(userCommandHandler);

            var profileCommandHandler = new ProfileCommandHandler(domainRepository, dateTimeProvider, userService, placeFinder, elevationService);
            commandDispatcher.RegisterHandler<CreateProfile>(profileCommandHandler);
            commandDispatcher.RegisterHandler<ChangeChart>(profileCommandHandler);
            commandDispatcher.RegisterHandler<RegisterProfileView>(profileCommandHandler);
            commandDispatcher.RegisterHandler<GiveKudos>(profileCommandHandler);
            commandDispatcher.RegisterHandler<DeleteProfile>(profileCommandHandler);

            return commandDispatcher;
        }

        private IEnumerable<Action<object>> CreatePostExecutionPipe(IEnumerable<Action<object>> postExecutionPipe)
        {
            if (postExecutionPipe != null)
            {
                foreach (var action in postExecutionPipe)
                {
                    yield return action;
                }
            }
        }
    }
}
