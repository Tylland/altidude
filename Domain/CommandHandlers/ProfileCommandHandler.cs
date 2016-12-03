using Altidude.Contracts.Commands;
using Altidude.Domain.Aggregates;
using System;

namespace Altidude.Domain.CommandHandlers
{
    internal class ProfileCommandHandler : IHandle<CreateProfile>, IHandle<ChangeChart>, IHandle<AddProfilePlace>, IHandle<RegisterProfileView>, IHandle<DeleteProfile>
    {
        private IDomainRepository _domainRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private IUserService _userService;
        private IPlaceFinder _placeFinder;

        public ProfileCommandHandler(IDomainRepository domainRepository, IDateTimeProvider dateTimeProvider, IUserService userService, IPlaceFinder placeFinder)
        {
            _domainRepository = domainRepository;
            _dateTimeProvider = dateTimeProvider;
            _userService = userService;
            _placeFinder = placeFinder;
        }

        public IAggregate Handle(CreateProfile command)
        {
            var user = _userService.GetById(command.UserId);

            return ProfileAggregate.Create(command.Id, user, command.Name, command.Track, _dateTimeProvider, _placeFinder);
        }

        public IAggregate Handle(ChangeChart command)
        {
            var aggregate = _domainRepository.GetById<ProfileAggregate>(command.Id);

            aggregate.ChangeChart(command.ChartId, command.Base64Image);

            return aggregate;
        }
        public IAggregate Handle(AddProfilePlace command)
        {
            var aggregate = _domainRepository.GetById<ProfileAggregate>(command.Id);

            aggregate.AddPlace(command.Name, command.Distance, command.Size, command.Split);

            return aggregate;
        }
        public IAggregate Handle(RegisterProfileView command)
        {
            var aggregate = _domainRepository.GetById<ProfileAggregate>(command.Id);

            aggregate.RegisterView(command.Referrer);

            return aggregate;
        }

        public IAggregate Handle(DeleteProfile command)
        {
            var user = _userService.GetById(command.UserId);

            var aggregate = _domainRepository.GetById<ProfileAggregate>(command.Id);

            aggregate.Delete(user);

            return aggregate;
        }
    }
}
