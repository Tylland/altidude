using System;
using System.Linq;
using Altidude.Contracts;
using Altidude.Contracts.Commands;
using Altidude.Contracts.Events;
using System.Collections.Generic;

namespace Altidude.Domain.EventHandlers
{
    public class UserProgressManager : IHandleEvent<UserCreated>, IHandleEvent<ProfileCreated>, IHandleEvent<ProfileDeleted>, IHandleEvent<ProfileViewRegistred>
    {
        private static readonly List<string> _socialRefferers = new List<string> { "facebook", "twitter", "shared" };

        private DomainEntry _domainEntry;

        public UserProgressManager(DomainEntry domainEntry)
        {
            _domainEntry = domainEntry;
        }

        private bool IsSocial(string referrer)
        {
            return _socialRefferers.Any(socialName => referrer.Contains(socialName));
        }

        public void Handle(ProfileViewRegistred evt)
        {
            if (IsSocial(evt.Referrer))
            {
                var points = evt.NrOfViews == 1 ? 10 : 1;

                _domainEntry.ExecuteCommand(new RegisterUserExperience(evt.UserId, points));
            }
        }

        public void Handle(ProfileCreated evt)
        {
            _domainEntry.ExecuteCommand(new RegisterUserExperience(evt.UserId, 5));
        }

        public void Handle(UserCreated evt)
        {
            _domainEntry.ExecuteCommand(new RegisterUserExperience(evt.Id, 10));
        }

        public void Handle(ProfileDeleted evt)
        {
            _domainEntry.ExecuteCommand(new RegisterUserExperience(evt.UserId, -5));
        }
    }
}
