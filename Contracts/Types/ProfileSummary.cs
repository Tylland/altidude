using System;

namespace Altidude.Contracts.Types
{
    public class ProfileSummary
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public int NrOfViews { get; set; }
        public int Kudos { get; set; }
        public DateTime CreatedTime { get; set; }

        public ProfileSummary(Guid id, Guid userId, string name, int nrOfViews, int kudos, DateTime createdTime)
        {
            Id = id;
            UserId = userId;
            Name = name;
            NrOfViews = nrOfViews;
            Kudos = kudos;
            CreatedTime = createdTime;
        }
    }
}
