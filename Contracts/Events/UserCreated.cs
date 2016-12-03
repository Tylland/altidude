using System;

namespace Altidude.Contracts.Events
{
    public class UserCreated : IEvent
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedTime { get; set; }

        public UserCreated(Guid id, string userName, string email, string firstName, string lastName, DateTime createdTime)
        {
            Id = id;
            UserName = userName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;

            CreatedTime = createdTime;
        }
        public UserCreated()
        {

        }
    }
}
