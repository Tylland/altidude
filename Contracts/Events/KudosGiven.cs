using Altidude.Contracts.Types;
using System;
using Newtonsoft.Json;

namespace Altidude.Contracts.Events
{
    public class KudosGiven : IEvent
    {
        public Guid Id { get; set; }
        public Guid OwnerUserId { get; set; }
        public Guid UserId { get; set; }
        public int TotalKudos { get; set; }

        public KudosGiven(Guid profileId, Guid ownerUserId, Guid userId, int totalKudos)
        {
            Id = profileId;
            OwnerUserId = ownerUserId;
            UserId = userId;
            TotalKudos = totalKudos;
        }
        public KudosGiven()
        {

        }
    }
}
