using System;

namespace Altidude.Contracts.Commands
{
    public class RegisterUserExperience : ICommand
    {
        public Guid Id { get; set; }
        public int Points { get; set; }

        public RegisterUserExperience(Guid id, int points)
        {
            Id = id;
            Points = points;
        }
        public RegisterUserExperience()
        {

        }
    }
}
