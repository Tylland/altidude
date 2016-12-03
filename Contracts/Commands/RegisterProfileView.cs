using System;

namespace Altidude.Contracts.Commands
{
    public class RegisterProfileView : ICommand
    {
        public Guid Id { get; set; }
        public string Referrer { get; set; }

        public RegisterProfileView(Guid id, string referrer)
        {
            Id = id;
            Referrer = referrer;
        }

        public RegisterProfileView()
        {

        }
    }
}
