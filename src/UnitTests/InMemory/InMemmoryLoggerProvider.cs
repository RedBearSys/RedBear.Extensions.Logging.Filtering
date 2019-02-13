using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace UnitTests.InMemory
{
    class InMemmoryLoggerProvider : ILoggerProvider
    {
        private readonly ICollection<LoggedItem> _cache;

        public InMemmoryLoggerProvider(ICollection<LoggedItem> cache)
        {
            _cache = cache;
        }

        public void Dispose()
        {
            _cache.Clear();
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new InMemoryLogger(_cache);
        }
    }
}
