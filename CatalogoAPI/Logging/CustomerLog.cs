using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace CatalogoAPI.Logging
{
    public class CustomerLog : ILogger
    {
        readonly string loggerName;
        readonly CustomLogProviderConfiguration loggerConfig;

        public CustomerLog(string name, CustomLogProviderConfiguration config)
        {
            loggerName = name;
            loggerConfig = config;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception exception, Func<TState, Exception, string> formatter)
        {
            string mensagem = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";

            EscreverTextoNoArquivo(mensagem);
        }

        private void EscreverTextoNoArquivo(string mensagem)
        {
            string caminhoArquivoLog = @"c:\dados\log\CATALOGOAPI_log.txt";
            using (StreamWriter streamWriter = new StreamWriter(caminhoArquivoLog, true))
            {
                try
                {
                    streamWriter.WriteLine(mensagem);
                    streamWriter.Close();
                }
                catch (Exception e)
                {
                    throw new System.Exception($"{e.Message}");
                }
            }
        }
    }
}