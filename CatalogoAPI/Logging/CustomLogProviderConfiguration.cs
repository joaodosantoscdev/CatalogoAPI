using Microsoft.Extensions.Logging;

namespace CatalogoAPI.Logging
{
    public class CustomLogProviderConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        public int EventId { get; set; } = 0;
        
             
    }
}