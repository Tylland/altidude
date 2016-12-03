using System;

namespace Altidude.Contracts.Commands
{
    public class CreateUser : ICommand
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public CreateUser(Guid id, string userName, string email, string firstName, string lastName)
        {
            Id = id;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
        }

        public CreateUser()
        {

        }
    }
}
