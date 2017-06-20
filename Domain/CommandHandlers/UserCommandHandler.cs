using Altidude.Contracts.Commands;
using Altidude.Domain.Aggregates;

namespace Altidude.Domain.CommandHandlers
{
    internal class UserCommandHandler : IHandle<CreateUser>, IHandle<RegisterUserExperience>, IHandle<UpdateUserSettings>, IHandle<FollowUser>, IHandle<UnfollowUser>, IHandle<ClearFollowingUsers>
    {
        private readonly IDomainRepository _domainRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserLevelService _levelService;

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

        public IAggregate Handle(FollowUser command)
        {
            var aggregate = _domainRepository.GetById<UserAggregate>(command.Id);

            aggregate.Follow(command.OtherUserId);

            return aggregate;
        }

        public IAggregate Handle(UnfollowUser command)
        {
            var aggregate = _domainRepository.GetById<UserAggregate>(command.Id);

            aggregate.Unfollow(command.OtherUserId);

            return aggregate;
        }
        public IAggregate Handle(ClearFollowingUsers command)
        {
            var aggregate = _domainRepository.GetById<UserAggregate>(command.Id);

            aggregate.ClearFollowingUsers();

            return aggregate;
        }
    }
}


    