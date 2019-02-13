using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using System;
using System.Collections.Generic;

namespace UnitTests.InMemory
{
    class InMemoryLogger : ILogger
    {
        private readonly ICollection<LoggedItem> _cache;

        public InMemoryLogger(ICollection<LoggedItem> cache)
        {
            _cache = cache;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _cache.Add(new LoggedItem
            {
                Level = logLevel,
                Value = state as FormattedLogValues
            });
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new InMemoryScope();
        }
    }
}
