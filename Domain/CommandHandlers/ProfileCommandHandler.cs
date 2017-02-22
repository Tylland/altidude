using Altidude.Contracts.Commands;
using Altidude.Domain.Aggregates;
using Altidude.Domain.Aggregates.Profile;
using System;

namespace Altidude.Domain.CommandHandlers
{
    internal class ProfileCommandHandler : IHandle<CreateProfile>, IHandle<ChangeChart>, IHandle<AddProfilePlace>, IHandle<RegisterProfileView>, IHandle<GiveKudos>, IHandle<DeleteProfile>
    {
        private IDomainRepository _domainRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private IUserService _userService;
        private IPlaceFinder _placeFinder;
        private IElevationService _elevationService;

        public ProfileCommandHandler(IDomainRepository domainRepository, IDateTimeProvider dateTimeProvider, IUserService userService, IPlaceFinder placeFinder, IElevationService elevationService)
        {
            _domainRepository = domainRepository;
            _dateTimeProvider = dateTimeProvider;
            _userService = userService;
            _placeFinder = placeFinder;
            _elevationService = elevationService;
        }

        public IAggregate Handle(CreateProfile command)
        {
            var user = _userService.GetById(command.UserId);

            return ProfileAggregate.Create(command.Id, user, command.Name, command.Track, _dateTimeProvider, _placeFinder, _elevationService);
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

        public IAggregate Handle(GiveKudos command)
        {
            var user = _userService.GetById(command.UserId);

            var aggregate = _domainRepository.GetById<ProfileAggregate>(command.Id);

            aggregate.GiveKudos(user);

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
