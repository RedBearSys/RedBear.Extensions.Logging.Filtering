using Microsoft.Extensions.Logging;

namespace RedBear.Extensions.Logging.Filtering
{
    public class FilteringLoggerProvider<TProvider> : ILoggerProvider where TProvider : ILoggerProvider
    {
        private readonly ILoggerProvider _providerToFilter;
        private readonly FilteringLogger.FilterMessage _filter;

        public FilteringLoggerProvider(ILoggerProvider providerToFilter, FilteringLogger.FilterMessage filter)
        {
            _providerToFilter = providerToFilter;
            _filter = filter;
        }
 
        public void Dispose()
        {
            _providerToFilter?.Dispose();
        }

        public ILogger CreateLogger(string categoryName)
        {
            var internalLogger = _providerToFilter.CreateLogger(categoryName);
            return new FilteringLogger(internalLogger, _filter);
        }
    }
}
