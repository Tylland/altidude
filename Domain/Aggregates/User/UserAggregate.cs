using Altidude.Contracts.Events;
using Altidude.Contracts.Types;
using System;

namespace Altidude.Domain.Aggregates
{
    public class UserAggregate : AggregateBase
    {
        public string UserName { get; private set; }
        public string Email { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public int ExperiencePoints { get; private set; }
        public UserLevel Level { get; private set; }
        public bool AcceptsEmails { get; private set; }

        public UserAggregate()
        {
            RegisterTransition<UserCreated>(Apply);
            RegisterTransition<UserGainedExperience>(Apply);
        }

        public UserAggregate(Guid id, string userName, string email, string firstName, string lastName, DateTime time)
            : this()
        {
            if(userName.Contains("@"))
            {
                var parts = userName.Split('@');

                if (parts.Length > 0)
                    userName = parts[0];
            }

            RaiseEvent(new UserCreated(id, userName, email, firstName, lastName, time));
        }
        public void Apply(UserCreated evt)
        {
            Id = evt.Id;
            UserName = evt.UserName;
            Email = evt.Email;
            FirstName = evt.FirstName;
            LastName = evt.LastName;
            Level = new UserLevel(0, 0, 0);
        }
        public void Apply(UserGainedExperience evt)
        {
            ExperiencePoints = evt.TotalPoints;
        }
        public void Apply(UserSettingsUpdated evt)
        {
            FirstName = evt.FirstName;
            LastName = evt.LastName;
            AcceptsEmails = evt.AcceptsEmails;
        }
        public static IAggregate Create(Guid id, string userName, string email, string firstName, string lastName, DateTime time)
        {
            return new UserAggregate(id, userName, email, firstName, lastName, time);
        }
        public void RegisterExperience(IUserLevelService levelService, int points)
        {
            var totalPoints = ExperiencePoints + points;

            RaiseEvent(new UserGainedExperience(Id, points, totalPoints));

            var level = levelService.CalcLevel(totalPoints);

            if (level.Level != Level.Level)
                RaiseEvent(new UserGainedLevel(Id, level));

        }
        public void UpdateSettings(string firstName, string lastName, bool acceptsEmails)
        {
            RaiseEvent(new UserSettingsUpdated(Id, firstName, lastName, acceptsEmails));
        }
    }
}
