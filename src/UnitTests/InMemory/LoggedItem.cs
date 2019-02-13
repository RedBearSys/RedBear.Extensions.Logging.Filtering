using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;

namespace UnitTests.InMemory
{
    class LoggedItem
    {
        public LogLevel Level { get; set; }
        public FormattedLogValues Value { get; set; }
    }
}
