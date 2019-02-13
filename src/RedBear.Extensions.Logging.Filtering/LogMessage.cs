using Microsoft.Extensions.Logging;
using System;

namespace RedBear.Extensions.Logging.Filtering
{
    /// <summary>The details that are about to be send to an ILogger implementation. Provides an opportunity for the message to be changed or ignored.</summary>
    public class LogMessage
    {
        /// <summary>The log level.</summary>
        /// <value>The log level.</value>
        public LogLevel LogLevel { get; set; }
        /// <summary>Gets the event identifier.</summary>
        /// <value>The event identifier.</value>
        public EventId EventId { get; }
        /// <summary>The state.</summary>
        /// <value>The state.</value>
        public object State { get; set; }
        /// <summary>Gets the exception.</summary>
        /// <value>The exception.</value>
        public Exception Exception { get; }
        /// <summary>Gets or sets a value indicating whether this <see cref="LogMessage"/> should be ignored and not sent to the logger.</summary>
        /// <value>
        ///   <c>true</c> if ignore; otherwise, <c>false</c>.</value>
        public bool Ignore { get; set; }

        public LogMessage(LogLevel logLevel, EventId eventId, object state, Exception exception)
        {
            LogLevel = logLevel;
            EventId = eventId;
            State = state;
            Exception = exception;
        }
    }
}
