using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace UnitTests.InMemory
{
    static class InMemoryLoggerExtensions
    {
        public static ILoggingBuilder AddInMemory(this ILoggingBuilder builder, ICollection<LoggedItem> cache)
        {
            builder.AddProvider(new InMemmoryLoggerProvider(cache));
            return builder;
        }
    }
}
