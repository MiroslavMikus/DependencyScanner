using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;

namespace DependencyScanner.Standalone.Services
{
    public class EventSink : ILogEventSink
    {
        internal IFormatProvider FormatProvider { get; set; }

        public event EventHandler<string> NotifyEvent;

        public EventSink(IFormatProvider formatProvider)
        {
            FormatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            if (logEvent.Level == LogEventLevel.Error || logEvent.Level == LogEventLevel.Fatal && NotifyEvent != null)
            {
                var message = logEvent.RenderMessage(FormatProvider);

                NotifyEvent?.Invoke(this, message);
            }
        }
    }

    public static class EventSinkExtensions
    {
        public static LoggerConfiguration EventSink(this LoggerSinkConfiguration loggerConfiguration, IFormatProvider formatProvider = null, EventSink sink = null)
        {
            if (formatProvider != null && sink != null)
            {
                sink.FormatProvider = formatProvider;
            }

            return loggerConfiguration.Sink(sink ?? new EventSink(formatProvider));
        }
    }
}
