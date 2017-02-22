using Altidude.Contracts.Types;
using System;

namespace Altidude.Contracts.Events
{
    public class KudosGiven : IEvent
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public int TotalKudos { get; set; }

        public KudosGiven(Guid profileId, Guid userId, int totalKudos)
        {
            Id = profileId;
            UserId = userId;
            TotalKudos = totalKudos;
        }
        public KudosGiven()
        {

        }
    }
}
