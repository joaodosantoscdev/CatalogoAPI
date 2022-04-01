using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using CatalogoAPI.Logging;

namespace CatalogoAPI.Logging
{
  public class CustomLogProvider : ILoggerProvider
  {
      readonly CustomLogProviderConfiguration _loggerConfig;
      readonly ConcurrentDictionary<string, CustomerLog> loggers = 
           new ConcurrentDictionary<string, CustomerLog>();
           public CustomLogProvider(CustomLogProviderConfiguration loggerConfig)
           {
               _loggerConfig = loggerConfig;
           }
    public ILogger CreateLogger(string categoryName)
    {
      return loggers.GetOrAdd(categoryName, name => new CustomerLog(categoryName, _loggerConfig));
    }

    public void Dispose()
    {
      loggers.Clear();
    }
  }
}