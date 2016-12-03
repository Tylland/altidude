using System;
using Altidude.Domain;

namespace Altidude.Infrastructure
{
    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime Now
        {
            get
            {
                return DateTime.Now;
            }
        }
    }
}
