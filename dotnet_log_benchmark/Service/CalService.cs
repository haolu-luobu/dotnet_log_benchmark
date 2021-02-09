using dotnet_log_benchmark.Controllers;
using Microsoft.Extensions.Logging;

namespace dotnet_log_benchmark.service
{
    public class CalService
    {
        private static ILogger<CalService> _logger;

        public CalService(ILogger<CalService> logger )
        {
            _logger = logger;
        }

        public int DoMath()
        {
            _logger.LogInformation($"test this hashcode: [{this.GetHashCode()}]");
            _logger.LogInformation($"test log hashcode: [{_logger.GetHashCode()}]");
            return 111;
        }
    }
}
