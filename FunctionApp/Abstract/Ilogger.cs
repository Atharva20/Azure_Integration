namespace AzureIntegration.Logger
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    public interface ILoggerWrapper
    {
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
        // Add other logging methods if needed
    }
    [ExcludeFromCodeCoverage]
    public class LoggerWrapper : ILoggerWrapper
    {
        private readonly ILogger _logger;

        public LoggerWrapper(ILogger logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning(message);
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }
    }
}