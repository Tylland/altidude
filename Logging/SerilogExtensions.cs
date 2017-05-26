using System;
using Serilog;

namespace Altidude.Logging
{
    public static class SerilogExtensions
    {
        public static TimingLogger StartTiming(this ILogger logger, string messageTemplate,
            params object[] propertyValues)
        {
            return new TimingLogger(logger, true, TimeSpan.Zero, messageTemplate, propertyValues);
        }
        public static TimingLogger StartTiming(this ILogger logger, bool enabled, string messageTemplate,
            params object[] propertyValues)
        {
            return new TimingLogger(logger, enabled, TimeSpan.Zero, messageTemplate, propertyValues);
        }
    }
}
