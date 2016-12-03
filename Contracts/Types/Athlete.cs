using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Contracts.Types
{
    public class Athlete
    {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }

        public Athlete(Guid userId, string displayName)
        {
            UserId = userId;
            DisplayName = displayName;
        }
        public Athlete()
        {

        }

    }
}
