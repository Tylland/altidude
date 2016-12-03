using System;

namespace Altidude.Contracts.Events
{
    public class UserSettingsUpdated : IEvent
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool AcceptsEmails { get; set; }

        public UserSettingsUpdated(Guid id, string firstName, string lastName, bool acceptsEmails)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            AcceptsEmails = acceptsEmails;
        }
        public UserSettingsUpdated()
        {

        }
    }
}
