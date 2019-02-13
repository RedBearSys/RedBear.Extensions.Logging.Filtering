using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace RedBear.Extensions.Logging.Filtering
{
    public static class FilteringLoggerExtensions
    {
        /// <summary>  Adds the filtered logger to the ILoggingBuilder.</summary>
        /// <param name="builder">The builder.</param>
        /// <param name="config">  Use this to add the logging providers you want to filter.</param>
        /// <param name="filter">  A function that allows you to examine or transform the logger message before it's sent to a logger implementation.</param>
        /// <returns></returns>
        public static ILoggingBuilder AddFilteredLogger(this ILoggingBuilder builder,
            Action<ILoggingBuilder> config,
            FilteringLogger.FilterMessage filter)
        {
            // Collect the desired logging provider configurations
            var hostBuilder = new HostBuilder()
                .ConfigureLogging(config);

            var host = hostBuilder.Build();

            // Find the logging providers from the dependency injection container.
            var providers = host.Services.GetServices<ILoggerProvider>();

            foreach (var provider in providers)
            {
                // Create a FilteredLoggerProvider wrapper for each
                // provider registered.
                var genericType = typeof(FilteringLoggerProvider<>);
                Type[] typeArgs = { provider.GetType() };
                var finalType = genericType.MakeGenericType(typeArgs);
                var finalProvider = (ILoggerProvider) Activator.CreateInstance(finalType, provider, filter);
                builder.Services.AddSingleton(finalProvider);
            }

            return builder;
        }
    }
}
