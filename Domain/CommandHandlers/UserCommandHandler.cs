using Altidude.Contracts.Commands;
using Altidude.Domain.Aggregates;
using System;

namespace Altidude.Domain.CommandHandlers
{
    internal class UserCommandHandler : IHandle<CreateUser>, IHandle<RegisterUserExperience>, IHandle<UpdateUserSettings>
    {
        private IDomainRepository _domainRepository;
        private IDateTimeProvider _dateTimeProvider;
        private IUserLevelService _levelService;

        public UserCommandHandler(IDomainRepository domainRepository, IDateTimeProvider dateTimeProvider, IUserLevelService levelService)
        {
            _domainRepository = domainRepository;
            _dateTimeProvider = dateTimeProvider;
            _levelService = levelService;
        }

        public IAggregate Handle(CreateUser command)
        {
            return UserAggregate.Create(command.Id, command.UserName, command.Email, command.FirstName, command.LastName, _dateTimeProvider.Now);
        }

        public IAggregate Handle(RegisterUserExperience command)
        {
            var aggregate = _domainRepository.GetById<UserAggregate>(command.Id);

            aggregate.RegisterExperience(_levelService, command.Points);

            return aggregate;
        }

        public IAggregate Handle(UpdateUserSettings command)
        {
            var aggregate = _domainRepository.GetById<UserAggregate>(command.Id);

            aggregate.UpdateSettings(command.FirstName, command.LastName, command.AcceptsEmails);

            return aggregate;
        }
    }
}


    