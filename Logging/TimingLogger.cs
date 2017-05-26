using System;
using Serilog;

namespace Altidude.Logging
{
    public class TimingLogger : IDisposable
    {
        private static int _instances;
        private readonly ILogger _logger;
        private readonly bool _enabled;
        private readonly string _exitMessageTemplate;
        private readonly DateTime _startTime;

        private bool WriteDebug { get; set; }
        private TimeSpan WarningThreshold { get; set; }
        private TimeSpan ErrorThreshold { get; set; }


        public TimingLogger WithWarning(long milliseconds)
        {
            return WithWarning(TimeSpan.FromMilliseconds(milliseconds));
        }

        public TimingLogger WithWarning(TimeSpan warningThreshold)
        {
            WarningThreshold = warningThreshold;

            return this;
        }

        public TimingLogger WithError(long milliseconds)
        {
            return WithError(TimeSpan.FromMilliseconds(milliseconds));
        }

        public TimingLogger WithError(TimeSpan threshold)
        {
            ErrorThreshold = threshold;

            return this;
        }

        public TimingLogger WithDebug()
        {
            WriteDebug = true;

            return this;
        }


        internal TimingLogger(ILogger logger, bool enabled, TimeSpan warningThreshold, string messageTemplate, object[] propertyValues)
        {
            _logger = logger;
            _enabled = enabled;

            WarningThreshold = warningThreshold;

            messageTemplate = ">> " + messageTemplate;

            if (_enabled)
            {
                _logger.Debug(messageTemplate.PadLeft(messageTemplate.Length + _instances * 2, ' '), propertyValues);

                _exitMessageTemplate = "<< " + string.Format(messageTemplate, propertyValues) +
                                       " - Elapsed Time: {ElapsedTime}";

                _instances++;
            }

            _startTime = DateTime.Now;
        }

        //internal TimingLogger(ILogger logger, TimeSpan warningThreshold, string pluginNamn, string eventId, string messageTemplate, object[] propertyValues)
        //{
        //    _logger = logger;

        //    WarningThreshold = warningThreshold;

        //    var eventTemplate = ">> <{PluginNamn:l}> <{EventId:l}> " + messageTemplate;
        //    var eventPropertyValues = new object[] { pluginNamn, eventId }.Concat(propertyValues).ToArray();

        //    _logger.Debug(eventTemplate.PadLeft(eventTemplate.Length + _instances * 2, ' '), eventPropertyValues);

        //    _exitMessageTemplate = "<< <" + pluginNamn + "> <" + eventId + "> - Elapsed Time: {ElapsedTime}";

        //    _instances++;

        //    _startTime = DateTime.Now;
        //}

        ~TimingLogger()
        {
            Dispose(false);
        }

        public bool IsDisposed { get; private set; }
        protected void Dispose(bool isDisposing)
        {
            if (!IsDisposed)
            {
                if (_enabled)
                {
                    var elapsedTime = DateTime.Now.Subtract(_startTime);

                    var padding = Math.Max(_instances--, 0);

                    if (ErrorThreshold != TimeSpan.Zero && elapsedTime > ErrorThreshold)
                        _logger.Error(string.Empty.PadLeft(padding, ' ') + _exitMessageTemplate, elapsedTime);
                    else if (WarningThreshold != TimeSpan.Zero && elapsedTime > WarningThreshold)
                        _logger.Warning(string.Empty.PadLeft(padding, ' ') + _exitMessageTemplate, elapsedTime);


                    if (WriteDebug)
                        _logger.Debug(string.Empty.PadLeft(padding, ' ') + _exitMessageTemplate, elapsedTime);
                }

                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
