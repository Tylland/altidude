using Altidude.Contracts.Types;
using System;

namespace Altidude.Contracts.Events
{
    public class ProfileViewRegistred : IEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Referrer { get; set; }
        public int NrOfViews { get; set; }

        public ProfileViewRegistred(Guid id, Guid userId, string referrer, int nrOfViews)
        {
            Id = id;
            UserId = userId;
            Referrer = referrer;
            NrOfViews = nrOfViews;
        }
        public ProfileViewRegistred()
        {

        }
    }
}
