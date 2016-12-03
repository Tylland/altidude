using Altidude.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altidude.Infrastructure
{
    public class ManualDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now { get; set; }
    }
}
