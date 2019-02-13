using Microsoft.Extensions.Logging;
using System;
using Microsoft.Extensions.Logging.Internal;

namespace RedBear.Extensions.Logging.Filtering
{
    public class FilteringLogger : ILogger
    {
        private readonly ILogger _innerLogger;
        private readonly FilterMessage _filter;

        public FilteringLogger(ILogger innerLogger, FilterMessage filter)
        {
            _innerLogger = innerLogger;
            _filter = filter;
        }

        public delegate void FilterMessage(LogMessage message);

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            LogMessage message;

            if (state is FormattedLogValues)
            {
                var lv = (FormattedLogValues)(object) state;
                message = new LogMessage(logLevel, eventId, lv.Clone(), exception);
            }
            else
            {
                message = new LogMessage(logLevel, eventId, state, exception);
            }

            _filter(message);

            if (state.GetType() != message.State.GetType())
            {
                // Or formatter won't work!
                throw new InvalidOperationException(
                    $"The original state was of type {state.GetType().FullName} but this has been changed to type {message.State.GetType().FullName}. The type of the state must not change.");
            }

            if (!message.Ignore)
                _innerLogger.Log(message.LogLevel, message.EventId, (TState)message.State, message.Exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _innerLogger.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _innerLogger.BeginScope(state);
        }
    }
}
