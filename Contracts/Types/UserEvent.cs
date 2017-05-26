using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Contracts.Types
{
    public class UserEvent
    {
        public Guid UserId { get; set; }
        public DateTime Time { get; set; }
        public string TypeName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ObjectTypeName { get; set; }
        public Guid ObjectId { get; set; }

    }
}
