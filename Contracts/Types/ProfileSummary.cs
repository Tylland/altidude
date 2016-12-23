using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Contracts.Types
{
    public class ProfileSummary
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public int NrOfViews { get; set; }
        public DateTime CreatedTime { get; set; }

        public ProfileSummary(Guid id, Guid userId, string name, int nrOfViews, DateTime createdTime)
        {
            Id = id;
            UserId = userId;
            Name = name;
            NrOfViews = nrOfViews;
            CreatedTime = createdTime;
        }
    }
}
