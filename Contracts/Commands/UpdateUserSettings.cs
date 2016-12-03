using System;

namespace Altidude.Contracts.Commands
{
    public class UpdateUserSettings : ICommand
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool AcceptsEmails { get; set; }

        public UpdateUserSettings(Guid id, string firstName, string lastName, bool acceptsEmails)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            AcceptsEmails = acceptsEmails;
        }

        public UpdateUserSettings()
        {

        }
    }
}
