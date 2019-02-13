using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Internal;
using RedBear.Extensions.Logging.Filtering;
using System.Collections.Generic;
using UnitTests.InMemory;
using Xunit;

namespace UnitTests
{
    public class FilterTests
    {
        [Fact]
        public void FilteredSuccessfully()
        {
            var filteredCache = new List<LoggedItem>();
            var unfilteredCache = new List<LoggedItem>();

            var builder = new HostBuilder()
                .ConfigureLogging(options =>
                {
                    options.AddFilteredLogger(filteredOptions =>
                    {
                        // Add logging providers to filter - e.g. AddConsole()
                        // This uses a testing logging provider that stores logged
                        // entries in an ICollection.
                        filteredOptions.AddInMemory(filteredCache);
                    }, message =>
                    {
                        // This action will be run for every logged message
                        // for each of the providers registered above.
                        if (!(message.State is FormattedLogValues lv)) return;

                        if (lv.ToString() == "foo")
                        {
                            // Change the value from "foo" to "bar"
                            message.State = lv.SetSimpleValue("bar");
                        }
                    });

                    // Unfiltered loggers can be registered here
                    options.AddInMemory(unfilteredCache);

                });

            var host = builder.Build();
            var logger = host.Services.GetService<ILogger<FilterTests>>();

            logger.LogWarning("blah");
            logger.LogWarning("foo");

            // Success outcomes:
            //  * both filtered and unfiltered have "blah" logged;
            //  * only unfiltered has "foo" logged;
            //  * the filtered logger has "bar" logged instead since it was transformed
            //    by the action above.

            Assert.Contains(filteredCache, x => x.Value.ToString() == "blah");
            Assert.Contains(unfilteredCache, x => x.Value.ToString() == "blah");

            Assert.Contains(filteredCache, x => x.Value.ToString() == "bar");
            Assert.Contains(unfilteredCache, x => x.Value.ToString() == "foo");

            Assert.DoesNotContain(unfilteredCache, x => x.Value.ToString() == "bar");
            Assert.DoesNotContain(filteredCache, x => x.Value.ToString() == "foo");
        }

        [Fact]
        public void IgnoredSuccessfully()
        {
            var filteredCache = new List<LoggedItem>();
            var unfilteredCache = new List<LoggedItem>();

            var builder = new HostBuilder()
                .ConfigureLogging(options =>
                {
                    options.AddFilteredLogger(filteredOptions =>
                    {
                        // Add logging providers to filter
                        filteredOptions.AddInMemory(filteredCache);
                    }, message =>
                    {
                        if (!(message.State is FormattedLogValues lv)) return;

                        if (lv.ToString() == "foo")
                        {
                            message.Ignore = true;
                        }
                    });

                    // Unfiltered loggers can be registered here
                    options.AddInMemory(unfilteredCache);

                });

            var host = builder.Build();
            var logger = host.Services.GetService<ILogger<FilterTests>>();

            logger.LogWarning("blah");
            logger.LogWarning("foo");

            Assert.Contains(filteredCache, x => x.Value.ToString() == "blah");
            Assert.Contains(unfilteredCache, x => x.Value.ToString() == "blah");

            Assert.Contains(unfilteredCache, x => x.Value.ToString() == "foo");
            Assert.DoesNotContain(filteredCache, x => x.Value.ToString() == "foo");
        }
    }
}
