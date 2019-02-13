# RedBear.Extensions.Logging.Filtering
Allows `ILogger` messages to be transformed or filtered based on its *content* before being passed on to other logger provider implementations.

## Example Use-Case

Our original use-case for this library was to handle personally-identifying information in logs. 

One of our applications processes bank transactions and we needed to log bank account numbers to local log files so our end-customer can check the system is working correctly or address any business issues that might arise.

As a service provider, we also stream the applications logs to the Cloud so we can monitor the software remotely for system problems. However, we didn't want full bank account numbers being transmitted.

This library allowed us to log full account numbers to a *local* log file for the end-customer, but allowed us to use a `Regex` to scrub bank account details from logs that were streamed to the cloud.

## Usage

Using an `ILoggingBuilder` instance, you can call `AddFilteredLogger(ILoggingBuilder, Filter)` and using the *inner* `ILoggingBuilder`, you then register the `Microsoft.Extensions.Logging` providers you want to filter. The second parameter allows you to specify an action to perform on a message before it's passed on to the logger itself.

Here's an example:

```csharp
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

```

## Transforming Messages

### Log Level

You can change the `LogLevel` of a message using the `LogMessage.Level` property.

### Message Content

This class used to store the actual message content is *typically* (but not always) an instance of a `Microsoft.Extensions.Logging.Internal.FormattedLogValues` class. This is known as the "state" and is stored in the `LogMessage.State` property of each message.

The purpose of the class is to allow logging of both a basic message *and* structured data. Unfortunately, the class isn't so straightforward to handle, so we've added a few extension methods to make life easier.

Consider the following code:

```csharp
var lv = new FormattedLogValues("The first number is {first} and the second is {second}", 10.ToString(), 20.ToString());
```

For getting values:

* calling `lv.ToString()` will result in the text `The first number is 10 and the second is 20`. 
* calling `lv.GetOriginalValue()` will return the text `The first number is {first} and the second is {second}`.
* calling `lv.GetValues()` will return the array `[ "10", "20" ]`.

For changing values, a few other convenient extension methods have been added:

* `.Clone()` will create a copy of the original `FormattedLogValues` instance;
* `.SetValues(params object[] values)` will change the values stored - e.g. 10 and 20 could be changed to 20 and 30;
* `.SetSimpleValue(string value)` can be used to create a `FormattedLogValues` instance based on a simple string.

## Ignoring Messages

This is really easy; just set `LogMessage.Ignore` to be `true` and it won't be passed on to the logger.